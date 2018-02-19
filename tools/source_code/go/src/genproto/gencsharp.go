package genproto

import (
	"bytes"
	"fmt"
	"html/template"
	"io/ioutil"
	"log"
	"os"
	"os/exec"
	"path/filepath"
	"strings"

	"config"
)

func GenCsharp() {
	if config.Conf.Proto.CsharpDir == "" {
		return
	}

	GenCsharpProto()

	FixCsharpProtoNamespace()

	GenCsharpInterface()

	GenCsharpProtoPatch()

	GenCsharpAppFnMapping()
}

func GenCsharpProto() {
	log.Print("Generating c sharp proto files...")

	eachProtoFn(func(path, dir, fileName string) {
		csharpDir := fmt.Sprintf("%s/%s", config.Conf.Proto.CsharpDir, dir)
		err := os.MkdirAll(csharpDir, os.ModePerm)
		if err != nil {
			panic(err)
		}

		csharpOut := fmt.Sprintf("-o:%s/%s.pb.cs", csharpDir, fileName)
		protoFilePath := fmt.Sprintf("-i:%s", path)

		cmd := exec.Command(config.Conf.Proto.MonoCmd, config.Conf.Proto.CsharpProtoGenCmd, protoFilePath, csharpOut)

		cmd.Stdout = os.Stdout
		cmd.Stderr = os.Stderr

		if err := cmd.Run(); err != nil {
			log.Fatalf("Gen Go proto ERROR: %v\n", err)
		}

		if !cmd.ProcessState.Success() {
			log.Fatalf("Gen Go proto ERROR: %s\n", cmd.ProcessState)
		}
	})
}

func FixCsharpProtoNamespace() {
	filepath.Walk(config.Conf.Proto.CsharpDir, func(path string, info os.FileInfo, err error) error {
		if info.IsDir() {
			return nil
		}

		if strings.LastIndex(info.Name(), ".pb.cs") == -1 {
			return nil
		}

		fileContent, err := ioutil.ReadFile(path)
		if err != nil {
			log.Fatal("Could not open c sharp proto file: ", err)
		}

		fileContent = namespaceFilter(fileContent)

		err = ioutil.WriteFile(path, fileContent, 0644)
		if err != nil {
			log.Fatal("refresh ruby proto error: ", err)
		}

		return nil
	})
}

func namespaceFilter(fileContent []byte) []byte {
	i := bytes.Index(fileContent, []byte("namespace"))
	if i == -1 {
		return fileContent
	}

	var fileBuf bytes.Buffer
	fileBuf.Grow(len(fileContent) + 1024)

	fileBuf.Write(fileContent[0:i])
	fileBuf.Write([]byte("namespace ProtoVO {\n"))
	fileBuf.Write(fileContent[i:])
	fileBuf.Write([]byte("\n}"))
	return fileBuf.Bytes()
}

var genCsharpInterfaceTmpl = `
// This file is auto-generated. DO NOT EDIT!

using System;
public interface IProtocolHead
{
}
`

func GenCsharpInterface() {
	tmpl, err := template.New("genCsharpInterfaceTmpl").Parse(genCsharpInterfaceTmpl)
	if err != nil {
		log.Fatal("parse c sharp interface file error: ", err)
	}

	writerBuf := new(bytes.Buffer)
	tmpl.Execute(writerBuf, nil)

	filePath := fmt.Sprintf("%s/IProtocolHead.cs", config.Conf.Proto.CsharpDir)
	os.Remove(filePath)
	err = ioutil.WriteFile(filePath, writerBuf.Bytes(), 0644)
	if err != nil {
		log.Fatal("Write c sharp app patch error: ", err)
	}
}

var csharpProtoPatchTmpl = `
// This file is auto-generated. DO NOT EDIT!


using System;
namespace ProtoVO
{
	namespace {{.AppName}}
	{
		public partial class {{.Name}} : IProtocolHead
		{
			public static UInt16 GetAppCode() {
				return {{.AppCode}};
			}

			public static UInt16 GetFnCode() {
				return {{.FnCode}};
			}

			public static byte GetFlag(){
				return {{.Flag}};
			}

			public static string GetUrl(){
				return "{{.Url}}";
			}

			public static string GetHttpMethod() {
				return "{{.HttpMethod}}";
			}
		}
	}
}
`

func GenCsharpProtoPatch() {
	tmpl, err := template.New("csharpProtoPatchTmpl").Parse(csharpProtoPatchTmpl)
	if err != nil {
		log.Fatal("parse c sharp proto patch file error: ", err)
	}

	for _, msg := range appFns {

		if msg.FnCode == -1 {
			continue
		}

		writerBuf := new(bytes.Buffer)
		tmpl.Execute(writerBuf, msg)

		filePath := fmt.Sprintf("%s/%s/%s.patch.cs", config.Conf.Proto.CsharpDir, msg.AppName, msg.UnderScoreName)
		os.Remove(filePath)
		err := ioutil.WriteFile(filePath, writerBuf.Bytes(), 0644)
		if err != nil {
			log.Fatal("Write c sharp app patch error: ", err)
		}
	}
}

var csharpAppFnMappingTmpl = `
// This file is auto-generated. DO NOT EDIT!

using System;
public class AppFnMapping {
	public static Type[] MAPPING = new Type[{{.FnLen}}] {
		{{range $i, $f := .Msgs}}{{if $i}},{{end}}{{if $f}}typeof(ProtoVO.{{$f.AppName}}.{{$f.Name}}){{else}}null{{end}}
		{{end}}
		};
}
`

type AppFnMapping struct {
	AppLen int
	FnLen  int
	Msgs   []*Message
}

func GenCsharpAppFnMapping() {
	tmpl, err := template.New("csharpAppFnMappingTmpl").Parse(csharpAppFnMappingTmpl)
	if err != nil {
		log.Fatal("parse c sharp app fn mapping file error: ", err)
	}
	writerBuf := new(bytes.Buffer)

	fnLen := len(msgs)

	var newMsgs = make([]*Message, fnLen)
	for fnIdx, fns := range msgs {
		newMsgs[fnIdx] = fns
	}

	tmpl.Execute(writerBuf, &AppFnMapping{AppLen: 0, FnLen: fnLen, Msgs: newMsgs})

	err = ioutil.WriteFile(fmt.Sprintf("%s/AppFnMapping.cs", config.Conf.Proto.CsharpDir), writerBuf.Bytes(), 0644)
	if err != nil {
		log.Fatal("Write c sharp app fn mapping error: ", err)
	}
}
