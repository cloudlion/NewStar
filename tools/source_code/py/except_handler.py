# -*- coding:utf-8 -*-
import sys
import traceback

def _excepthook(exctype, value, tb):
    print "Exception Caught!"
    print "type: " + str(exctype)
    print "value: " + str(value)
    #print 'you get an unknown exception, contact author zhang shi liang!'
    traceback.print_tb(tb)
    #os.system('pause')
    raw_input("press Enter to continue...")
    exit()

def _error_and_exit(msg):
    print "Error! %s\n" % msg
    raw_input("press Enter to exit...")
    exit()
    
def _error_and_continue(msg):
    print "Error! %s\n" % msg
    raw_input("press Enter to continue...")
    
