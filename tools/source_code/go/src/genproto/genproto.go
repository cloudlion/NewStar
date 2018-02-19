package genproto

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"path/filepath"
	"regexp"
	"strconv"

	"config"
	"utils"
)

var (
	maxAppCode int
	maxFnCode  int
	apps       map[string]*App
	appCodes   map[int]*App
	appFns     map[int]*Message
	msgs       []*Message
	meta       *Meta
)

var (
	eachProtoFn    func(func(path, dir, fileName string))
	eachProtoDirFn func(func(dir string))
)

func Init() {
	readMeta()
	initEachProtoFn()
	initEachProtoDirFn()

	initApps()

	initAppFns()

	//refreshMetaFile()
}

type Meta struct {
	AppCodes map[string]int
	FnCodes  map[string]int
}

func readMeta() {
	path := fmt.Sprintf("%s/meta.json", config.Conf.Proto.ProtoDir)
	data, err := ioutil.ReadFile(path)
	if err != nil {
		log.Fatal("read meta file error: ", err)
	}

	meta = &Meta{}
	err = json.Unmarshal(data, meta)
	if err != nil {
		log.Fatal("parse meta file error: ", err)
	}
}

func initEachProtoDirFn() {
	eachProtoDirFn = func(fn func(dir string)) {
		dirs, err := ioutil.ReadDir(config.Conf.Proto.ProtoDir)
		if err != nil {
			panic(err)
		}

		for _, dir := range dirs {
			if dir.IsDir() {
				fn(dir.Name())
			}
		}
	}
}

func initEachProtoFn() {
	eachProtoFn = func(fn func(path, dir, fileName string)) {
		filepath.Walk(config.Conf.Proto.ProtoDir, func(path string, info os.FileInfo, err error) error {
			if info.IsDir() || filepath.Ext(info.Name()) != ".proto" {
				return nil
			}

			dir := filepath.Base(filepath.Dir(path))

			fileName := filepath.Base(path)
			fileName = fileName[0 : len(fileName)-len(".proto")]

			fn(path, dir, fileName)
			return nil
		})
	}
}

func initApps() {
	apps = make(map[string]*App)
	appCodes = make(map[int]*App)

	for appName, appCode := range meta.AppCodes {
		app := new(App)

		app.AppCode = appCode
		app.AppName = appName
		app.CalmAppName = utils.UnderScore2Calm(app.AppName)
		apps[appName] = app
		appCodes[app.AppCode] = app

		if app.AppCode > maxAppCode {
			maxAppCode = app.AppCode
		}
	}
}

func initAppFns() {
	appFns = make(map[int]*Message)

	eachProtoFn(func(path, dir, fileName string) {
		fileContent, err := ioutil.ReadFile(path)
		if err != nil {
			log.Fatal("Could not open proto file: ", err)
		}
		msgs := splitMessages(fileContent)

		for _, msg := range msgs {

			msg.AppName = dir
			msg.UnderScoreName = utils.Calm2UnderScore(msg.Name)
			msg.CalmAppName = utils.UnderScore2Calm(msg.AppName)
			if code, ok := meta.FnCodes[msg.Name]; ok {
				msg.FnCode = code
			} else {
				msg.FnCode = -1
			}

			appFns[msg.FnCode] = msg

			if msg.FnCode > maxFnCode {
				maxFnCode = msg.FnCode
			}
		}
	})

	msgs = make([]*Message, maxFnCode+1)
	for i := 0; i < int(maxFnCode)+1; i++ {
		msgs[i] = appFns[int(i)]
	}
}

//func refreshMetaFile() {
//	appCodeInfos := make(map[string]string)
//	for _, app := range apps {
//		appCodeInfos[fmt.Sprintf("%d", app.AppCode)] = app.AppName
//	}

//	fnCodeInfos := make(map[string]map[string]string)
//	for appCode, fns := range appFns {
//		fcs := make(map[string]string)
//		app := appCodes[appCode]
//		fnCodeInfos[app.AppName] = fcs

//		for fnCode, msg := range fns {
//			if fnCode == -1 {
//				continue
//			}

//			fcs[fmt.Sprintf("%d", fnCode)] = msg.Name
//		}
//	}

//	output := make(map[string]interface{})
//	output["AppCodes"] = appCodeInfos
//	output["FnCodes"] = fnCodeInfos

//	data, err := json.MarshalIndent(output, "", "\t")
//	if err != nil {
//		log.Fatal("Gen meta file error: ", err)
//	}

//	fileName := fmt.Sprintf("%s/meta.json", config.Conf.Proto.ProtoDir)
//	os.Remove(fileName)
//	err = ioutil.WriteFile(fileName, data, 0644)
//	if err != nil {
//		log.Fatal("Write meta file error: ", err)
//	}
//}

func splitMessages(data []byte) []*Message {
	msgs := make([]*Message, 0)
	for msg, idx := findNextMessage(data); msg != nil; msg, idx = findNextMessage(data) {
		msgs = append(msgs, msg)
		data = data[idx:]
	}
	return msgs
}

func findNextMessage(data []byte) (*Message, int) {
	idx := bytes.Index(data, []byte("message"))
	if idx == -1 {
		return nil, -1
	}

	msg := &Message{}
	pair := 0
	nextIdx := bytes.Index(data[idx:], []byte("{"))
	if nextIdx == -1 {
		log.Fatal("invalid proto file")
	}

	nextLeftIdx := nextIdx
	nextRightIdx := nextIdx
	pair++
	offset := idx + nextIdx + 1

	for {
		if pair == 0 {
			break
		}
		nextRightIdx = bytes.Index(data[offset:], []byte("}"))

		if nextRightIdx == -1 {
			log.Fatal("invalid proto file")
		}

		nextLeftIdx = bytes.Index(data[offset:], []byte("{"))
		if nextLeftIdx == -1 {
			pair--
			offset = offset + nextRightIdx + 1
			continue
		}

		if nextRightIdx < nextLeftIdx {
			pair--
			offset = offset + nextRightIdx + 1
			continue
		}

		pair++
		offset = offset + nextLeftIdx + 1
	}

	msgBlock := data[idx+7 : offset]

	fnRe := regexp.MustCompile(`//\s*fn\s*=\s*([\d]+)`)
	fnMatch := fnRe.FindSubmatch(msgBlock)

	if len(fnMatch) > 0 {
		msg.FnCode = int(utils.Atoi64(string(fnMatch[1])))
	} else {
		msg.FnCode = -1
	}

	flagRe := regexp.MustCompile(`//\s*flag\s*=\s*([\d]+)`)
	flagMatch := flagRe.FindSubmatch(msgBlock)

	if len(flagMatch) > 0 {
		flag, err := strconv.ParseUint(string(flagMatch[1]), 10, 8)
		if err != nil {
			log.Fatal("invalid fn code: ", err)
		}
		msg.Flag = uint8(flag)
	}

	urlRe := regexp.MustCompile(`//\s*url\s*=\s*(.+)`)
	urlMatch := urlRe.FindSubmatch(msgBlock)
	if len(urlMatch) > 0 {
		msg.Url = string(urlMatch[1])
	}

	httpMethodRe := regexp.MustCompile(`//\s*httpMethod\s*=\s*(.+)`)
	httpMethodMatch := httpMethodRe.FindSubmatch(msgBlock)
	if len(httpMethodMatch) > 0 {
		msg.HttpMethod = string(httpMethodMatch[1])
	}

	msg.Name = string(bytes.TrimSpace(data[idx+7 : nextIdx+idx]))

	if msg.FnCode == 2 && msg.Name == "CsCollectResource" {
		log.Printf("%s", data)
	}

	return msg, nextIdx
}

type App struct {
	AppCode     int
	AppName     string
	CalmAppName string
	MaxFnCode   int
}

type Message struct {
	CalmAppName    string
	AppCode        int
	AppName        string
	Name           string
	UnderScoreName string
	FnCode         int
	Flag           uint8
	Url            string
	HttpMethod     string
}
