package main

import (
	"flag"
	"log"

	"mm.tools/config"
	"mm.tools/gengds"
)

func main() {
	log.SetFlags(log.Ldate | log.Ltime | log.Lshortfile)

	var configFile string
	flag.StringVar(&configFile, "c", "proto.toml", "The proto config file")
	flag.Parse()

	config.ParseConfig(configFile)

	gengds.GenGo()
}
