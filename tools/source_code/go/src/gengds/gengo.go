package gengds

import (
	"bytes"
	"encoding/csv"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"os/exec"
	"reflect"
	"strings"
	"text/template"

	"mm.tools/config"
	"mm.tools/utils"
)

func GenGo() {
	for _, gdsListField := range config.Conf.Gds.List {
		GenGoForFile(gdsListField.File, gdsListField.Keys)
	}

	formatGoFiles()

	log.Print("gen go gds success")
}

func formatGoFiles() {
	cmd := exec.Command("gofmt", "-w", config.Conf.Gds.GoDir)

	cmd.Stdout = os.Stdout
	cmd.Stderr = os.Stderr

	if err := cmd.Run(); err != nil {
		log.Fatalf("Fmt Go file ERROR: ", err, cmd)
	}

	if !cmd.ProcessState.Success() {
		log.Fatalf("Fmt Go file ERROR: %s\n", cmd.ProcessState)
	}
}

type gdsMeta struct {
	Name            string
	Keys            []string
	KeyMapping      map[string]string
	KeyMapping2     map[string]string
	Fields          []gdsField
	GdsStructFields []gdsStructField
}

var goGdsMultiTmpl = `
// This file is auto-generated. DO NOT EDIT!
package gds

import (
	"encoding/csv"
	"fmt"
	"os"
	"log"
	"strings"
	"hd/connector"
	"mm/common"
)

var {{.Name | calm | lowerFirst}}s {{.Name | calm}}s

func {{.Name | calm}}(realmId int64) {{.Name | calm}}s {
	return {{.Name | calm | lowerFirst}}s
}

{{$keyMapping := .KeyMapping}}
{{$keyMapping2 := .KeyMapping2}}
{{$fields := .Fields}}
{{$keys := .Keys}}
{{$gdsStructFields := .GdsStructFields}}
{{$name := .Name}}
{{range $i, $key := .Keys}}
	{{if last $i $keyMapping}}
type {{index $keyMapping $key | calm}}s []*{{$name | calm}}Data

func (o {{index $keyMapping $key | calm}}s) {{$key | calm}}({{$key | calm | lowerFirst}} int64) *{{$name | calm}}Data {
	return []*{{$name | calm}}Data(o)[{{$key | calm | lowerFirst}}]
}
	{{else}}
type {{index $keyMapping $key | calm}}s []{{$key | calm}}{{index $keyMapping $key | calm}}s

func (o {{index $keyMapping $key | calm}}s) {{$key | calm}}({{$key | calm | lowerFirst}} int64) {{$key | calm}}{{index $keyMapping $key | calm}}s {
	return []{{$key | calm}}{{index $keyMapping $key | calm}}s(o)[{{$key | calm | lowerFirst}}]
}
	{{end}}
{{end}}

type {{.Name | calm}}Data struct {
	{{range $idx, $field := .Fields}}
	{{$field.Name | calm | lowerFirst}} {{$field.Cls | fieldType}}
	{{end}}
}
{{range $idx, $field := .Fields}}
func (o *{{$name | calm}}Data) {{$field.Name | calm}} () {{$field.Cls | fieldType}} {
	return o.{{$field.Name | calm | lowerFirst}}
}
{{end}}

{{range $i, $st := .GdsStructFields}}
type {{$name | calm}}{{$st.StName}} struct {
	{{range $j, $f := $st.Fields}}
	{{$f | calm | lowerFirst}} int64
	{{end}}
}

	{{range $j, $f := $st.Fields}}
		func (d {{$name | calm}}{{$st.StName}}) {{$f | calm}} () int64 {
			return d.{{$f | calm | lowerFirst}}
		}
	{{end}}

{{end}}

type {{$name | calm}}Meta struct {
	name string
	idx int64
	cls string
}

func init(){
	log.SetFlags(log.Ldate | log.Ltime | log.Lshortfile)
	go func() {
		<-connector.ConfLoadCh

		file, err := os.Open(fmt.Sprintf("%s/%s", connector.GdsDir(), "{{.Name}}.csv"))
		if err != nil {
			log.Fatal("could not read gds csv file: ", err)
		}

		defer func() {
			if err := file.Close(); err != nil {
				log.Fatal("could not close gds csv file: ", err)
			}
		}()

		{{$name | calm | lowerFirst}}ParseCsv(file)
	}()
}

func {{$name | calm | lowerFirst}}ParseCsv(file *os.File) {
	reader := csv.NewReader(file)
	reader.Comma = ','
	records, err := reader.ReadAll()
	if err != nil {
		log.Fatal("read gds csv file error: ", err)
	}

	{{$name | calm | lowerFirst}}ParseRows(records[0], records[1], records[2], records[3:])
}

func {{$name | calm | lowerFirst}}ParseRows(fields, types, targets []string, datas [][]string) {
	metas := {{$name | calm | lowerFirst}}ParseMetas(fields, types, targets)
	{{$keyLen := len $keys}}
	{{if eq $keyLen 1}}
	targetDatas := make([]*{{$name | calm}}Data, 0)
	{{else}}
	targetDatas := make([]{{index $keys 0 | calm}}{{$name | calm}}s, 0)
	{{end}}

	for _, data := range datas {
		targetData := &{{.Name | calm}}Data{}
		{{range $i, $f := .Fields}}
			{{if $f.IsStruct}}
		{{$f.Name | calm | lowerFirst}} := make([]{{$name | calm}}{{$f.Name | calm | singular}}, 0)
		{{$f.Name | calm | lowerFirst}}S := data[metas["{{$f.Name|calm|lowerFirst}}"].idx]
		if len({{$f.Name | calm | lowerFirst}}S) > 0 {
			{{$f.Name | calm | lowerFirst | singular}}Strs := strings.Split({{$f.Name | calm | lowerFirst}}S, "|")
			for _, str := range {{$f.Name | calm | lowerFirst | singular}}Strs {
				temp := strings.Split(str, "*")
				for len(temp) < 2 {
					temp = append(temp, "0")
				}

				{{$f.Name | calm | lowerFirst}} = append({{$f.Name | calm | lowerFirst}}, {{$name | calm}}{{$f.Name | calm | singular}}{
					{{$fs := findStructField $gdsStructFields $f.Name}}
					{{range $k, $stf := $fs.Fields}}
					{{$stf | calm | lowerFirst}}: common.Atoi64(temp[{{$k}}]),
					{{end}}
				})
			}
		}
		
		targetData.{{$f.Name | calm | lowerFirst}} = {{$f.Name | calm | lowerFirst}}

			{{else}}
		targetData.{{$f.Name | calm | lowerFirst}} = {{fieldConvert $fields $f.Name}}(data[metas["{{$f.Name | calm | lowerFirst}}"].idx])
			{{end}}
			
		{{end}}
		targetDatas = {{$name | calm | lowerFirst}}AddData(targetData, targetDatas)
	}

	{{$name | calm | lowerFirst}}s = {{$name | calm}}s(targetDatas)
}

func {{$name | calm | lowerFirst}}ParseMetas(fields, types, targets []string) map[string]{{$name | calm}}Meta {
	metas := make(map[string]{{$name | calm}}Meta)
	for i, field := range fields {
		target := targets[i]
		if !strings.Contains(target, "s") {
			continue
		}

		field = common.DownFirstChar(common.UnderScore2Calm(strings.TrimSpace(field)))
		metas[field] = {{$name | calm}}Meta{
			name: field,
			idx:  int64(i),
			cls:  types[i],
		}
	}
	return metas
}

{{$keyLen := len $keys}}
{{if eq $keyLen 1}}
	func {{$name | calm | lowerFirst}}AddData(targetData *{{$name | calm}}Data, targetDatas []*{{$name | calm}}Data) []*{{$name | calm}}Data {
{{else}}
	func {{$name | calm | lowerFirst}}AddData(targetData *{{$name | calm}}Data, targetDatas []{{index $keys 0 | calm}}{{$name | calm}}s) []{{index $keys 0 | calm}}{{$name | calm}}s {
{{end}}
	{{range $i, $k := $keys}}
		{{if eq $keyLen 1}}
		for len(targetDatas) <= int(targetData.{{index $keys 0 | calm | lowerFirst}}) {
			targetDatas = append(targetDatas, &{{$name | calm}}Data{})
		}
		targetDatas[targetData.{{index $keys 0 | calm | lowerFirst}}] = targetData
		{{else}}
			{{if last $i $keys}}
				{{$k | calm | lowerFirst}}Datas := targetDatas[targetData.{{index $keyMapping2 $k | calm | lowerFirst}}]

				for len({{$k | calm | lowerFirst}}Datas) <= int(targetData.{{$k | calm | lowerFirst}}) {
					{{$k | calm | lowerFirst}}Datas = append({{$k | calm | lowerFirst}}Datas, &{{$name | calm}}Data{})

				}

				targetDatas[targetData.{{index $keyMapping2 $k | calm | lowerFirst}}] = {{$k | calm | lowerFirst}}Datas

				{{$k | calm | lowerFirst}}Datas[targetData.{{$k | calm | lowerFirst}}] = targetData
			{{else if eq $i 0}}
				for len(targetDatas) <= int(targetData.{{$k | calm | lowerFirst}}) {
					targetDatas = append(targetDatas, {{$k | calm}}{{$name | calm}}s{})
				}
			{{else}}
			{{end}}
		{{end}}
	{{end}}
	return targetDatas
}
`

var converterMap = map[string]string{
	"int":      "common.Atoi64",
	"string":   "string",
	"string[]": "common.Atoas",
	"int[]":    "common.Atoi64s",
	"bool":     "common.Atob",
}

var fieldTypeMapping = map[string]string{
	"string[]": "[]string",
	"int[]":    "[]int64",
	"int":      "int64",
}

func fieldConvert(fields []gdsField, field string) string {
	f := findField(fields, field)
	return converterMap[f.Cls]
}

func findField(fields []gdsField, field string) gdsField {
	for _, f := range fields {
		if f.Name == field {
			return f
		}
	}

	return gdsField{}
}

func findStructField(fields []gdsStructField, name string) gdsStructField {
	for _, f := range fields {
		if f.StName == utils.UnderScore2Calm(utils.Singular(name)) {
			return f
		}
	}

	return gdsStructField{}
}

func GenGoForFile(fileName string, keys []string) {
	tmpl := template.New("goGdsMultiTmpl")
	tmpl = tmpl.Funcs(template.FuncMap{
		"calm":            utils.UnderScore2Calm,
		"lowerFirst":      utils.DownFirstChar,
		"singular":        utils.Singular,
		"fieldConvert":    fieldConvert,
		"findStructField": findStructField,
		"fieldType": func(cls string) string {
			ft := fieldTypeMapping[cls]
			if ft == "" {
				return cls
			}

			return ft
		},
		"last": func(x int, a interface{}) bool {
			return x == reflect.ValueOf(a).Len()-1
		},
	})

	tmpl, err := tmpl.Parse(goGdsMultiTmpl)
	if err != nil {
		log.Fatal("parse go gds template error: ", err)
	}

	keyMapping := make(map[string]string)
	keyMapping2 := make(map[string]string)
	var keyWrapers []string
	for idx, key := range keys {
		keyWrapers = append(keyWrapers, key)

		if idx == 0 {
			keyMapping[key] = fileName
			keyMapping2[key] = fileName
		} else {
			keyMapping[key] = keyWrapers[idx-1] + "_" + fileName
			keyMapping2[key] = keyWrapers[idx-1]
		}
	}

	meta := gdsMeta{
		Name:        fileName,
		Keys:        keyWrapers,
		KeyMapping:  keyMapping,
		KeyMapping2: keyMapping2,
	}

	os.Mkdir(fmt.Sprintf("%s", config.Conf.Gds.GdsDir), os.ModePerm)

	file, err := os.Open(fmt.Sprintf("%s/%s.csv", config.Conf.Gds.GdsDir, fileName))
	if err != nil {
		log.Fatal("could not read gds csv file: ", err)
	}
	defer func() {
		if err := file.Close(); err != nil {
			log.Fatal("could not close gds csv file: ", err)
		}
	}()

	reader := csv.NewReader(file)
	reader.Comma = ','
	records, err := reader.ReadAll()
	if err != nil {
		log.Fatal("read gds csv file error: ", err)
	}
	readMeta(fileName, records[0], records[1], records[2], &meta)

	writerBuf := new(bytes.Buffer)

	tmpl.Execute(writerBuf, meta)

	goFileName := fmt.Sprintf("%s/%s.go", config.Conf.Gds.GoDir, fileName)
	os.Remove(goFileName)
	err = ioutil.WriteFile(goFileName, writerBuf.Bytes(), 0644)
	if err != nil {
		log.Fatal("Write go gds file error: ", err)
	}
}

type gdsField struct {
	Name     string
	Cls      string
	IsStruct bool
}

type gdsStructField struct {
	StName string
	Fields []string
}

func readMeta(fileName string, fields, clses, targets []string, meta *gdsMeta) {
	var gdsFields []gdsField
	var gdsStructFields []gdsStructField

	for idx, target := range targets {
		if !strings.Contains(target, "s") {
			continue
		}

		field := gdsField{fields[idx], clses[idx], false}

		if strings.HasPrefix(field.Cls, "s-") {
			field.Cls = fmt.Sprintf("[]%s%s", utils.UnderScore2Calm(fileName), utils.UnderScore2Calm(utils.Singular(field.Name)))

			cls := clses[idx]
			cls = cls[0 : len(cls)-2]
			gdsStructFields = append(gdsStructFields, gdsStructField{
				StName: utils.UnderScore2Calm(utils.Singular(field.Name)),
				Fields: strings.Split(cls, "-")[2:],
			})

			field.IsStruct = true
		}

		gdsFields = append(gdsFields, field)
	}

	meta.Fields = gdsFields
	meta.GdsStructFields = gdsStructFields
}
