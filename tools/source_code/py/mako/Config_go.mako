#-*- coding: utf-8 -*-
<%def name="read_func(prex, type, posx)">
<%
func = ''
if type.startswith("bool"):
    func = prex + "ReadBool" + posx
elif type.startswith("byte"):
    func = prex + "ReadByte" + posx
elif type.startswith("short"):
    func = prex + "ReadShort" + posx
elif type.startswith("int"):
    func = prex + "ReadInt" + posx
elif type.startswith("long"):
    func = prex + "ReadLong" + posx
elif type.startswith("float"):
    func = prex + "ReadFloat" + posx
elif type.startswith("double"):
    func = prex + "ReadDouble" + posx
elif type.startswith("string"):
    func = prex + "ReadString" + posx
%>
${func}
</%def>

<%def name="conv_type(type)"><%
func = ''
if type.startswith("float"):
    func = type.replace("float", "float32")
elif type.startswith("double"):
    func = type.replace("double", "float64")
elif type.startswith("long"):
	func = type.replace("long", "int64")
elif type.startswith("short"):
	func = type.replace("short", "int16")
else:
    func = type
return func
%></%def>

<%def name="type_of_arr(arr)">
<%
tp = arr.strip('[]')
%>
${tp}
</%def>

<%
last_item_index = 0
for i in range(len(client_list)):
    if client_list[i].count('s') > 0:
        last_item_index = i + 1
%>

<%
item_count = 0
for i in range(len(client_list)):
    if client_list[i].count('s') > 0:
        item_count += 1
%>

<%
has_array_item = False
for i in range(len(client_list)):
    if client_list[i].count('s') > 0:
        if arr_list[i]:
            has_array_item = True
            break
%>

package gamedata

import (
	"fmt"
	"mad/base"
)

type ${class_name.capitalize()} struct {
    % for i in range(len(client_list)):
        % if client_list[i].count('s') > 0:
    	${name_list[i].capitalize()} ${conv_type(type_list[i])}
        % endif
    % endfor
}

func (obj *${class_name.capitalize()}) init(\
% for i in range(last_item_index - 1):
% if client_list[i].count('s') > 0:
${name_list[i]} ${conv_type(type_list[i])}, \
% endif
% endfor
${name_list[last_item_index - 1]} ${conv_type(type_list[last_item_index - 1])} \
) {
% for i in range(last_item_index):
% if client_list[i].count('s') > 0:
obj.${name_list[i].capitalize()} = ${name_list[i]}
% endif
% endfor
}

var (
    ${class_name.capitalize()}_dic map[int]*${class_name.capitalize()}
    ${class_name.capitalize()}_array []*${class_name.capitalize()}
)

func ${class_name.capitalize()}_Init()  {
	 ${class_name.capitalize()}_dic = make(map[int]*${class_name.capitalize()})
	 ${class_name.capitalize()}_array = make([]*${class_name.capitalize()}, 0)
	 ${class_name.capitalize()}_LoadData();
}
    
func ${class_name.capitalize()}_LoadData()  {
	 rs := base.NewStreamReader("../gamedata/" + "${bin_file_name}")
	 if rs == nil {
	 	 fmt.Println("create stream reader failed: " + "${bin_file_name}")
	 	 return
	 }

	 /*int file_len = */rs.ReadInt();
	 flag := rs.ReadString();
	 if flag != "${class_name}" {
	 	 fmt.Println("invalid file flag" + flag)
		 return
	 }

	 col_cnt := rs.ReadShort()
	 if col_cnt != ${item_count} {
	 	 fmt.Println("col cnt invalid" + string(col_cnt) + " : " + string(${item_count}))
		 return
	 }

	 row_cnt := rs.ReadInt()
	 for i := 0; i < row_cnt; i++ {
	 	 ${class_name.capitalize()}_Add_Item(rs);
	 }

	 rs.Close()
}

func ${class_name.capitalize()}_Add_Item(rs *base.StreamReader)  {
	 % if has_array_item:
	 var arr_item_len_${class_name} int
	 % endif

	 % for i in range(last_item_index):
	 % if client_list[i].count('s') > 0:
	 % if arr_list[i]:

	 arr_item_len_${class_name} = rs.ReadShort()
	 ${name_list[i]} := make([]${conv_type(type_of_arr(type_list[i]))}, arr_item_len_${class_name})

	 for(int i = 0; i < arr_item_len_${class_name}; ++i)
	 	 ${name_list[i]}[i] = ${read_func("rs.", type_list[i], "();")}
	 % else:
	 ${name_list[i]} := ${read_func("rs.", type_list[i], "();")}
	 % endif
	 % endif
	 % endfor

	 new_obj_${class_name} := new(${class_name.capitalize()})
	 new_obj_${class_name}.init(\
	 % for i in range(last_item_index - 1):
	 % if client_list[i].count('s') > 0:
  	 ${name_list[i]}, \
	 % endif
	 % endfor
	 ${name_list[last_item_index - 1]}\
	 )

	if ${name_list[0]} == 0 {
		fmt.Println("invalid key: " + string(${name_list[0]}))
		return
	}

	if ${class_name.capitalize()}_dic[${name_list[0]}] != nil {
		fmt.Println("duplicate key: " + string(${name_list[0]}))
		return
	}

	${class_name.capitalize()}_dic[${name_list[0]}] = new_obj_${class_name};
	${class_name.capitalize()}_array = append(${class_name.capitalize()}_array,
											  new_obj_${class_name});
}
