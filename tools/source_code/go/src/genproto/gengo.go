package genproto

import (
	"bytes"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"os/exec"
	"path/filepath"
	//	"sort"
	"strings"
	"text/template"

	"config"
)

func GenGo() {
	if config.Conf.Proto.GoDir == "" {
		return
	}

	GenGoProto()

	GenGoAppPatch()

	//	GenGoProtoPatch()
	GenGoScErrorProtoPatch()
	GenGoScServerErrorProtoPatch()

	FixGoProto()
}

//fix go proto file import issues
func FixGoProto() {
	for appName, _ := range apps {
		dir := fmt.Sprintf("%v/%v", config.Conf.Proto.GoDir, appName)
		//		fmt.Println("//########## start processing ", dir)
		filepath.Walk(dir, func(path string, info os.FileInfo, err error) error {
			if strings.HasSuffix(info.Name(), ".pb.go") && !strings.HasSuffix(info.Name(), ".ph.pb.go") {
				//					fmt.Println("//------------ FILE: ", info.Name())

				fileContent, fError := ioutil.ReadFile(path)
				if fError != nil {
					return nil
				}

				// find out if there are issues need to fix
				var isNeedFixSelfImport bool = false
				var selfpkgName string = ""
				var isNeedFixOtherImport bool = false
				lines := strings.Split(string(fileContent), "\n")
				for idx, line := range lines {
					//						fmt.Println(line)
					if isNeedFixSelfImport && strings.Index(line, selfpkgName) > 0 {
						//							fmt.Println("//------------ Need fix self import!!! ")
						lines[idx] = strings.Replace(line, fmt.Sprintf("%s.", selfpkgName), "", -1)
					}
					if strings.HasPrefix(line, "import") {
						//fix self-import
						if !isNeedFixSelfImport && strings.Index(line, fmt.Sprintf("\"%s/", appName)) > 0 {
							isNeedFixSelfImport = true
							words := strings.Split(line, " ")
							selfpkgName = words[1]
							lines[idx] = ""
							//								fmt.Printf("//------------ Find self import!! |%v| --> |%v|\n", line, lines[idx])
						}
						// fix import-other-app
						for otherAppName, _ := range apps {
							if otherAppName == appName {
								break
							}

							if strings.Index(line, fmt.Sprintf("\"%s/", otherAppName)) > 0 {
								isNeedFixOtherImport = true
								words := strings.Split(line, " ")
								words[2] = fmt.Sprintf("\"mm/%v/p\"", otherAppName)
								lines[idx] = strings.Join(words, " ")
								//									fmt.Printf("//------------ Find import other app |%v| --> |%v|\n", line, lines[idx])
							}
						}
					}

					if strings.HasPrefix(line, "package") {
						lines[idx] = strings.Replace(line, appName, "p", 1)
					}
				}

				if isNeedFixOtherImport || isNeedFixSelfImport || true {
					fileContentNew := strings.Join(lines, "\n")
					ioutil.WriteFile(path, []byte(fileContentNew), 0644)
				}

			}

			os.Mkdir(fmt.Sprintf("%s/p", dir), os.ModePerm)
			if strings.HasSuffix(info.Name(), ".pb.go") && info.Name() != "app.ph.pb.go" {
				os.Rename(path, fmt.Sprintf("%s/p/%s", dir, info.Name()))
				//					fmt.Println("~~~~!!!!!!!!!!!!mvmvmvm ", path,)
			}
			return nil
		})
		//		fmt.Println("//########## finished processing ", dir, "\n")
	}
}

func GenGoProto() {
	log.Print("Generating go proto files...")

	goOut := fmt.Sprintf("--go_out=%s", config.Conf.Proto.GoDir)

	eachProtoDirFn(func(dir string) {
		protoDir := fmt.Sprintf("%s/*.proto", dir)
		cmdStr := fmt.Sprintf("%s %s %s", "protoc", goOut, protoDir)
		cmd := exec.Command("sh", "-c", cmdStr)

		cmd.Stdout = os.Stdout
		cmd.Stderr = os.Stderr

		if err := cmd.Run(); err != nil {
			log.Printf("ERROR: %s", cmd)
			log.Fatalf("Gen Go proto ERROR: %s", err)
		}

		if !cmd.ProcessState.Success() {
			log.Fatalf("Gen Go proto ERROR: %s\n", cmd.ProcessState)
		}
	})
}

var goAppPatchTmpl = `
// This file is auto-generated. DO NOT EDIT!

package {{.AppName}}

const AppCode = {{.AppCode}}

`

func GenGoAppPatch() {
	tmpl, err := template.New("goAppPatchTmpl").Parse(goAppPatchTmpl)
	if err != nil {
		log.Fatal("parse go app patch file error: ", err)
	}

	for _, app := range apps {
		writerBuf := new(bytes.Buffer)
		tmpl.Execute(writerBuf, app)

		err = os.MkdirAll(fmt.Sprintf("%s/%s", config.Conf.Proto.GoDir, app.AppName), os.ModePerm)
		if err != nil {
			log.Fatal("Could not create dir: ", err)
		}

		fileName := fmt.Sprintf("%s/%s/app.ph.pb.go", config.Conf.Proto.GoDir, app.AppName)
		os.Remove(fileName)
		err = ioutil.WriteFile(fileName, writerBuf.Bytes(), 0644)
		if err != nil {
			log.Fatal("Write go app patch error: ", err)
		}
	}
}

var goProtoPatchTmpl = `
// This file is auto-generated. DO NOT EDIT!

package p

import (
	"hd/connector"

	"code.google.com/p/goprotobuf/proto"
)

func (m *{{.Name}}) Msg() (*connector.Msg, error) {
	data, err := proto.Marshal(m)
	if err != nil {
		return nil, err
	}

	msg := connector.NewMsg()
	msg.App = {{.AppCode}}
	msg.Fn = {{.FnCode}}
	msg.Flag = {{.Flag}}
	msg.Data = data

	return msg, nil
}

func (m *{{.Name}}) AppCode() uint16 {
	return {{.AppCode}}
}

func (m *{{.Name}}) FnCode() uint16 {
	return {{.FnCode}}
}
`

//func GenGoProtoPatch() {
//	tmpl, err := template.New("goProtoPatchTmpl").Parse(goProtoPatchTmpl)
//	if err != nil {
//		log.Fatal("parse go proto patch file error: ", err)
//	}

//	for appCode, fns := range appFns {
//		app := appCodes[appCode]
//		if app == nil {
//			continue
//		}

//		os.RemoveAll(fmt.Sprintf("%s/%s/p", config.Conf.Proto.GoDir, app.AppName))

//		fnCodes := make([]int, len(fns))
//		fnCodesMap := make(map[int]*Message)
//		i := 0
//		for _, msg := range fns {
//			fnCodes[i] = msg.FnCode
//			fnCodesMap[msg.FnCode] = msg
//			i++
//		}
//		sort.Ints(fnCodes)

//		for _, fnCode := range fnCodes {
//			msg := fnCodesMap[fnCode]

//			if msg.FnCode == -1 {
//				continue
//			}

//			writerBuf := new(bytes.Buffer)
//			tmpl.Execute(writerBuf, msg)

//			filePath := fmt.Sprintf("%s/%s/%s.ph.pb.go", config.Conf.Proto.GoDir, msg.AppName, msg.UnderScoreName)
//			os.Remove(filePath)
//			err := ioutil.WriteFile(filePath, writerBuf.Bytes(), 0644)
//			if err != nil {
//				log.Fatal("Write go app patch error: ", err, filePath)
//			}

//			//Write Fn Code to app.ph.pb.go
//			code := fmt.Sprintf("const %sFnCode = %d", msg.Name, msg.FnCode)
//			fileName := fmt.Sprintf("%s/%s/app.ph.pb.go", config.Conf.Proto.GoDir, msg.AppName)
//			f, _ := os.OpenFile(fileName, os.O_APPEND|os.O_WRONLY, 0600)
//			_, _ = f.WriteString(code + "\n")
//			f.Close()
//		}

//	}
//}

var genGoScErrorProtoPatchTmpl = `
// This file is auto-generated. DO NOT EDIT!

package p

import "fmt"

func (m *ScError) Error() string {
	return fmt.Sprintf("CsAppCode=%d, CsFnCode=%d, Code=%d, Detail=%s", m.GetCsAppCode(), m.GetCsFnCode(), m.GetCode(), m.GetDetail())
}
`

func GenGoScErrorProtoPatch() {
	tmpl, err := template.New("genGoScErrorProtoPatchTmpl").Parse(genGoScErrorProtoPatchTmpl)
	if err != nil {
		panic(err)
	}

	writerBuf := new(bytes.Buffer)
	tmpl.Execute(writerBuf, nil)

	filePath := fmt.Sprintf("%s/%s/%s.ph.pb.go", config.Conf.Proto.GoDir, "common", "sc_error.error")
	os.Remove(filePath)
	err = ioutil.WriteFile(filePath, writerBuf.Bytes(), 0644)
	if err != nil {
		log.Fatal("Write go app patch error: ", err, filePath)
	}
}

var genGoScServerErrorProtoPatchTmpl = `
// This file is auto-generated. DO NOT EDIT!

package p

import "fmt"

func (m *ScServerError) Error() string {
	return fmt.Sprintf("%s\n\n%s", m.GetTitle(), m.GetStackTrace())
}
`

func GenGoScServerErrorProtoPatch() {
	tmpl, err := template.New("genGoScServerErrorProtoPatchTmpl").Parse(genGoScServerErrorProtoPatchTmpl)
	if err != nil {
		panic(err)
	}

	writerBuf := new(bytes.Buffer)
	tmpl.Execute(writerBuf, nil)

	filePath := fmt.Sprintf("%s/%s/%s.ph.pb.go", config.Conf.Proto.GoDir, "common", "sc_server_error.error")
	os.Remove(filePath)
	err = ioutil.WriteFile(filePath, writerBuf.Bytes(), 0644)
	if err != nil {
		log.Fatal("Write go app patch error: ", err, filePath)
	}
}
