package utils

import (
	"log"
	"strconv"
	"strings"
	"unicode"
)

func UnderScore2Calm(s string) string {
	words := strings.Split(s, "_")
	var newWords []string

	for _, word := range words {
		a := []rune(word)
		a[0] = unicode.ToUpper(a[0])
		newWords = append(newWords, string(a))
	}

	return strings.Join(newWords, "")
}

func Calm2UnderScore(s string) string {
	var words []string
	for i := 0; s != ""; s = s[i:] {
		i = strings.IndexFunc(s[1:], unicode.IsUpper) + 1
		if i <= 0 {
			i = len(s)
		}
		words = append(words, strings.ToLower(s[:i]))
	}
	return strings.Join(words, "_")
}

func Atoi64(str string) int64 {
	if str == "" {
		return 0
	}

	i, err := strconv.ParseInt(str, 10, 0)
	if err != nil {
		log.Fatal("invalid gds conf: ", err, str)
	}

	return i
}

func Atob(str string) bool {
	if str == "1" {
		return true
	}

	return false
}

func DownFirstChar(s string) string {
	cs := []rune(s)
	cs[0] = unicode.ToLower(cs[0])
	return string(cs)
}

func Singular(str string) string {
	if strings.HasSuffix(str, "es") {
		return str[0 : len(str)-2]
	}

	if strings.HasSuffix(str, "s") {
		return str[0 : len(str)-1]
	}

	return str
}
