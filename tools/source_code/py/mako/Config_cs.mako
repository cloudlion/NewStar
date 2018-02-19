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

<%def name="type_of_arr(arr)">
<%
tp = arr.strip('[]')
%>
${tp}
</%def>

<%
last_item_index = 0
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        last_item_index = i + 1
%>

<%
item_count = 0
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        item_count += 1
%>

<%
has_array_item = False
for i in range(len(client_list)):
    if client_list[i].count('c') > 0:
        if arr_list[i]:
            has_array_item = True
            break
%>

using System;
using System.Collections.Generic;
using UnityEngine;
using MadEngine;

namespace Mad
{
    public class ${class_name}
    {
        public static bool resLoaded = false;
        public static Dictionary<int, ${class_name}> dic = new Dictionary<int, ${class_name}>();
        public static List<${class_name}> array = new List<${class_name}>();

		% for i in range(len(struct_define_list)):
		  public class ${struct_define_list[i].name} {
		  	  % for j in range(len(struct_define_list[i].vals)):
			  	  public int ${struct_define_list[i].vals[j]};
			  % endfor
		  }
		% endfor

        % for i in range(len(client_list)):
            % if client_list[i].count('c') > 0:
        public readonly ${type_list[i].replace('-', '_')} ${name_list[i]};
            % endif
        % endfor

        public ${class_name}(\
            % for i in range(last_item_index - 1):
                 % if client_list[i].count('c') > 0:
    ${type_list[i].replace('-', '_')} ${name_list[i]}, \
                 % endif
            % endfor
    ${type_list[last_item_index - 1].replace('-', '_')} ${name_list[last_item_index - 1]}\
    )
        {
            % for i in range(last_item_index):
                 % if client_list[i].count('c') > 0:
            this.${name_list[i]} = ${name_list[i]};
                 % endif
            % endfor
        }

        public static void Init()
        {
            if(resLoaded) return;
            App.ResMgr.GetAssetAsync("${bin_file_name}",
                                    new AssetLoadListener(loader, "OnLoadFile"));
        }

        private class Loader {
        private void OnLoadFile(string name, AssetRef ar, int errno)
        {
            if(errno != 0)
            {
                LogWarning("invalid file: " + name);
                return;
            }

            TextAsset ta = ar.Asset as TextAsset;
            if(ta == null)
            {
                Logger.LogError("text asset is null");
                return;
            }
            byte[] data = ta.bytes;

            ReadStream rs = new ReadStream(data);
            /*int file_len = */rs.ReadInt();
            string flag = rs.ReadString();
            if(flag != "${class_name}")
            {
                LogWarning("invalid file flag" + flag);
                return;
            }

            int col_cnt = rs.ReadShort();
            if(col_cnt != ${item_count})
            {
                LogWarning("col cnt invalid" + col_cnt + " : " + ${item_count});
                return;
            }

            int row_cnt = rs.ReadInt();
            for(int i = 0; i < row_cnt; i++)
            {
                Add_Item(rs);
            }

            ${class_name}.resLoaded = true;
        }

        private void Add_Item(ReadStream rs)
        {
            % if has_array_item:
                int arr_item_len_${class_name};
            % endif

            % for i in range(last_item_index):
                % if client_list[i].count('c') > 0:
                    % if arr_list[i]:

            arr_item_len_${class_name} = rs.ReadShort();
            ${type_list[i].replace('-', '_')} ${name_list[i]} = new ${type_of_arr(type_list[i].replace('-', '_'))} [arr_item_len_${class_name}];

            for(int i = 0; i < arr_item_len_${class_name}; ++i) {
					    % if struct_list[i].name:
				${name_list[i]}[i] = new ${type_of_arr(type_list[i].replace('-', '_'))}();
						% for val in struct_list[i].vals:
				${name_list[i]}[i].${val} = ${read_func("rs.", "int", "();")}
						% endfor
						% else:
                ${name_list[i]}[i] = ${read_func("rs.", type_list[i], "();")}
						% endif
			}
                    % else:
					    % if struct_list[i].name:
			${type_list[i].replace('-', '_')} ${name_list[i]} = new ${type_list[i].replace('-', '_')}();
						% for val in struct_list[i].vals:
			${name_list[i]}.${val} = ${read_func("rs.", "int", "();")}
						% endfor
						% else:
            ${type_list[i]} ${name_list[i]} = ${read_func("rs.", type_list[i], "();")}
						% endif
                    % endif
                % endif
            % endfor

            ${class_name} new_obj_${class_name} = new ${class_name}(\
            % for i in range(last_item_index - 1):
                 % if client_list[i].count('c') > 0:
    ${name_list[i]}, \
                 % endif
            % endfor
    ${name_list[last_item_index - 1]}\
    );
            if(${name_list[0]} == 0)
            {
                LogWarning("invalid key: " + ${name_list[0]});
                return;
            }
            if(${class_name}.dic.ContainsKey(${name_list[0]}))
            {
                LogWarning("duplicate key: " + ${name_list[0]});
                return;
            }

            ${class_name}.dic.Add(${name_list[0]}, new_obj_${class_name});
            ${class_name}.array.Add(new_obj_${class_name});
        }

        private void LogWarning(string msg)
        {
            Logger.LogWarning(msg);
        }
        }

        private static Loader loader = new Loader();
    }
}
