package main

import (
	"config"
	"flag"
	"genproto"
	"log"
	"strings"
)

func main() {
	log.SetFlags(log.Ldate | log.Ltime | log.Lshortfile)

	var configFile string
	var target string
	flag.StringVar(&configFile, "c", "proto.toml", "The proto config file")
	flag.StringVar(&target, "t", "go,ruby", "gen cs , ruby , go")
	flag.Parse()

	config.ParseConfig(configFile)

	genproto.Init()

	targets := strings.Split(target, ",")

	if has(targets, "go") {
		genproto.GenGo()
	}

	//	if has(targets, "ruby") {
	//		genproto.GenRuby()
	//	}

	if has(targets, "cs") {
		genproto.GenCsharp()
	}
}

func has(targets []string, target string) bool {
	for _, t := range targets {
		if strings.TrimSpace(t) == target {
			return true
		}
	}

	return false
}
