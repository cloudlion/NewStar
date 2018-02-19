package main

import (
	"fmt"
	"regexp"
	"strings"
)

func main() {
	msgBlock := `
##
# This file is auto-generated. DO NOT EDIT!
#
require 'protobuf/message'


##
# Imports
#
require 'common/test1.pb'
require 'common/test2.pb'

module Proto
module Common

  ##
  # Message Classes
  #
  class Test3 < ::Protobuf::Message; end


  ##
  # Message Fields
  #
  class Test3
    optional :string, :f1, 1
    optional :int32, :f2, 2
    optional ::Proto::Common::Test1, :test1, 3
    optional ::Proto::Common::Test2, :test2, 4
  end

end

end
	`

	ss := strings.Split(msgBlock, "\n")
	newSs := make([]string, len(ss))

	fnRe := regexp.MustCompile(`require\s'(.+).pb'`)

	for idx, s := range ss {
		fnMatch := fnRe.FindSubmatch([]byte(s))

		if len(fnMatch) > 0 {
			newSs[idx] = fmt.Sprintf("require 'proto/%s.pb'", fnMatch[1])
		} else {
			newSs[idx] = s
		}
	}

	fmt.Print(strings.Join(newSs, "\n"))
}
