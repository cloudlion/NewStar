# -*- coding:utf-8 -*-

"""generate res index.
    author: zhang shi liang
"""
import os
import sys
from pygraphviz import AGraph

def gen_graph_res_idx(exp_file, idx_map):
    keys = [key for key in idx_map.keys()]
    values = []
    for val in idx_map.values():
        if not val in values:
            values.append(val)

    G = AGraph(directed = True, strict = True)
    G.graph_attr['splines'] = 'true'
    G.graph_attr['overlap'] = 'false'
    #G.graph_attr['ordering'] = 'out'
    #G.graph_attr['concentrate'] = 'false'
    G.add_nodes_from(keys, color = 'green', style = 'filled')
    G.add_nodes_from(values, color = 'red', style = 'filled')
    G.add_edges_from(idx_map.items(), color = 'blue')
    G.layout(prog = GRAPH_PROG)
    G.draw(exp_file)

def parse_bundle_dep_file(file, map):
    fs = open(file, 'rt')
    if not fs:
        raise Exception, "can not open file: %s" % file

    lines = fs.readlines()
    for line in lines:
        key, value = line.split(':')
        key = key.strip()
        if key in map:
            raise Exception, "duplicate file found: %s" % key

        value = value.strip()
        vals = [val.strip() for val in value.split('|') if val]
        if not vals or not len(vals):
            print "warning! dep is empty for bundle: " + key
        map[key] = vals

def gen_graph_bundle_dep(exp_file, idx_map):
    G = AGraph(directed = True, strict = True)
    G.graph_attr['splines'] = 'true'
    G.graph_attr['overlap'] = 'false'
    #G.graph_attr['ordering'] = 'out'
    #G.graph_attr['concentrate'] = 'false'
    keys = [key for key in idx_map.keys()]
    values = []
    for lst in idx_map.values():
        for item in lst:
            if not item in values:
                values.append(item)
    kvp_list = []
    for key in idx_map.keys():
        for val in idx_map[key]:
            kvp_list.append((key, val))
    
    G.add_nodes_from(keys, color = 'green', style = 'filled')
    for val in values:
        if val.count('#exist'):
            G.add_node(val, color = 'yellow', style = 'filled')
        elif val.count('#miss'):
            G.add_node(val, color = 'red', style = 'filled');
    #G.add_nodes_from(values, color = 'red', style = 'filled')
    G.add_edges_from(kvp_list, color = 'blue')
    G.layout(prog = 'sfdp')
    G.draw(exp_file)

def parse_bundle_dep_file(file, map):
    fs = open(file, 'rt')
    if not fs:
        raise Exception, "can not open file: %s" % file

    lines = fs.readlines()
    for line in lines:
        key, value = line.split(':')
        key = key.strip()
        if key in map:
            raise Exception, "duplicate file found: %s" % key

        value = value.strip()
        vals = [val.strip() for val in value.split('|') if val]
        if not vals or not len(vals):
            print "warning! dep is empty for bundle: " + key
        map[key] = vals

def gen_bundle_dep(src_file, dst_file):
    bundle_dep_map = {}
    parse_bundle_dep_file(src_file, bundle_dep_map)
    gen_graph_bundle_dep(dst_file, bundle_dep_map)

if __name__ == '__main__':
    gen_bundle_dep(sys.argv[1], sys.argv[2])
