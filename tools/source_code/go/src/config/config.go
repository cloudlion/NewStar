package config

import (
	"io/ioutil"
	"log"

	"github.com/BurntSushi/toml"
)

var (
	Conf = &Config{}
)

type Config struct {
	Proto struct {
		ProtoDir          string
		RubyDir           string
		GoDir             string
		CsharpDir         string
		MonoCmd           string
		CsharpProtoGenCmd string
	}

	Gds struct {
		GdsDir string
		GoDir  string
		List   []GdsListField
	}
}

type GdsListField struct {
	File string
	Keys []string
}

func ParseConfig(configFile string) {
	configBytes, err := ioutil.ReadFile(configFile)
	if err != nil {
		log.Fatal("read config file error: ", err)
	}

	if _, err := toml.Decode(string(configBytes), Conf); err != nil {
		log.Fatal("parse config file error: ", err)
	}
}
