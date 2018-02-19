###########################
gen proto
###########################

brew install protobuf
gem install protobuf

go get code.google.com/p/goprotobuf/{proto,protoc-gen-go}
go get github.com/BurntSushi/toml

go run cmd/genproto/main.go -c config/config.toml -t go,ruby,cs




###########################
gen gds
###########################

go run cmd/gengds/main.go -c config/config.toml
