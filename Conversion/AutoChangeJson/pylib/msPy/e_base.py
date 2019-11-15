#encoding : utf-8

import os
import shutil
import struct
import codecs
import xlrd
import xls_reader
import operator
import string
import time
import datetime
from export_bin import *
from export_xml import *

ResType_Model		= 0
ResType_Skin		= 1
ResType_Map			= 2
ResType_Effect		= 3
ResType_ATK_Effect	= 4
ResType_Texture		= 5
ResType_Font		= 6
ResType_Sound		= 7
ResType_Mani		= 8

ResIDDict = [ dict(), dict(), dict(), dict(), dict(), dict(), dict(), dict(), dict()]
def GetResIDDict(type):
	return ResIDDict[type]

ResDict = [ dict(), dict(), dict(), dict(), dict(), dict(), dict(), dict(),	dict()]
def GetResDict(type):
	return ResDict[type]

#charconvertmap = { 'a': r'\a', 'b': r'\b', 'f': r'\f', 'n': r'\n', 'r': r'\r', '\\': r'/' }
charconvertmap = { 'n': r'\n', '\\': r'/' }
 
def strconvertor(s):
	flag = False

	result = []
	for c in s:
		if not flag and c == "\\":
			flag = True
		elif flag:
			flag = False
			if c in charconvertmap:
				result.append(charconvertmap.get(c, c))
			else:
				result.append('/')
				result.append(c)
		else:
			result.append(c)

	return ''.join(result)

def mkdir(p):
	p = p.strip()
	p = p.rstrip("\\")
	p = p.rstrip("/")
 
	exist = os.path.exists(p)
	if not exist:
		os.makedirs(p)

		return True
	else:
		return False
 
def create_file(fn):
	return codecs.open(fn, 'w', 'utf-8')

def create_fileA(fn):
	return codecs.open(fn, 'w', 'gb2312')

def prepair_path(fn):
	if os.path.isdir(fn):
		shutil.rmtree(fn)
	os.mkdir(fn)

def create_db_path(fn, name):
	item = os.path.split(fn)
	path = item[0] + '/' + name

	os.makedirs(path)

	return path

def create_db_file(fn, name):
	item = os.path.split(fn)
	f = create_file(item[0] + '/' + name)

	return f

def begin_generate_db_table(name):
	s = "----------------- auto generate db file ------------------------\n"
	s += "module(..., package.seeall)\n\n"
	s += "local require = require\n\n"
	s += "local " + name + " = \n{\n"

	return s

def begin_generate_db_table_pub(name):
	s = "----------------- auto generate db file ------------------------\n"
	s += name + " = \n{\n"

	return s

def end_generate_db_table(name):
	s = "\n};\n"
	s += "function get_db_table()\n"
	s += "\treturn " + name + ";\n"
	s += "end\n"

	return s
	
def end_generate_db_table_pub_filename(script):
	return "\tlocal filename = '"+ script + "' .. key\n\n"
	
def end_generate_dbtable():
	return "\t___loadedKeys = {},\n\n};\n"
	
def check_loaded_keys(keyName):
	s = "\tif table.___loadedKeys[" + keyName + "] ~= nil then\n"
	s += "\t\treturn nil;\n\tend\n"
	s += "\ttable.___loadedKeys[" + keyName + "] = true;\n\n"
	return s

def db_table_pub_begin(func_name):
	s = "local " + func_name + " = { __index  = function(table, key)\n"
	s += "\tif key <= 0 then\n"
	s += "\t\treturn nil;\n"
	s += "\tend\n\n"
	return s
	
def end_generate_db_table_pub_begin(func_name):
	return end_generate_dbtable() + db_table_pub_begin(func_name)

def end_generate_db_table_pub_functions():
	s = "\tlocal _call = function()\n"
	s += "\t\treturn require(filename).get_db_table();\n"
	s += "\tend\n\n"
	s += "\tlocal _err = function()\n"
	s += "\t\ti3k_warn('unable load db file[' .. filename .. ']');\n"
	s += "\t\t--i3k_warn(debug.traceback());\n"
	s += "\tend\n\n"

	return s
	
def end_generate_db_table_pub_call_functions():
	s = "\tlocal ret, _db = xpcall(_call, _err);\n"
	s += "\tif not ret then\n"
	s += "\t\treturn nil;\n"
	s += "\tend\n"
	
	return s
	
def db_table_pub_end(name, func_name):
	s = "\ttable[key] = _db;\n\n"
	s += "\treturn _db;\n"
	s += "end };\n"
	s += "setmetatable(" + name + ", " + func_name + ");\n\n"
	return s
	
def end_generate_db_table_pub_end(name, func_name):
	s = end_generate_db_table_pub_functions()
	s += end_generate_db_table_pub_call_functions()
	s += db_table_pub_end(name, func_name)
	return s
	
def end_generate_db_table_pub_end_with_unload_script(name, func_name):
	s = "\tfor k, v in pairs(_db) do\n\t\ttable[k] = v;\n\tend\n"
	s += "\ti3k_game_unload_script(filename);\n"
	s += "\treturn table[key]\n"
	s += "end };\n"
	s += "setmetatable(" + name + ", " + func_name + ");\n\n"
	return s

	#/////////////////////
def end_generate_db_table_pub_ex(name, script):
	s = end_generate_db_table_pub_begin("mt")
	s += end_generate_db_table_pub_filename(script)
	s += end_generate_db_table_pub_end(name, "mt")

	return s
	#//////////////////////////////


#################多语言相关导出函数 ##########################
def generate_meta_get_header():
	s = ""
	return s

# keyList: list of string
def generate_meta_table(dbName, keyList):
	s = ""
	return s

def generate_meta_get_footer(dbName):
	s = ""
	return s

def generate_meta_get_var_name(source):
	return source + ""

def writeFileInteger(integer):
	integer = str(integer)
	fileName = "temp.txt"
	with open(fileName, 'w') as f:
		f.write(integer)

def readFileInteger():
	fileName = "temp.txt"
	if(not os.path.exists(fileName)):
		print("=====================================\n")
		print("临时文件未找到：语言定义表.xlsx 的导出一定要在 图片索引路径表.xlsx、excelWord.xlsx、layers_widgets.xlsx、文本配置表.xlsx 之前导出！")
		print("\n=====================================")

	with open(fileName, 'r') as f:
		res = int(f.read())
		return res

def removeTempFile():
	fileName = "temp.txt"
	if(os.path.exists(fileName)):
		os.remove(fileName)
#########################################

def str_split(string, delimiters):
    if string.strip() == '':
        return []
    delimiters = tuple(delimiters)
    stack = [string, ]

    for delimiter in delimiters:
        for i, substring in enumerate(stack):
            substack = substring.split(delimiter)
            stack.pop(i)
            for j, _substring in enumerate(substack):
                stack.insert(i + j, _substring)

    return stack

def conv_int(c, default = 0):
	s = ''

	ss = str(c).lstrip().rstrip()
	if ss != '':
		s = str(int(float(ss)))

	if s == '':
		s = str(default)

	return s

def conv_flo(c, default = 0.0):
	s = ''

	ss = str(c).lstrip().rstrip()
	if ss != '':
		s = str(float(ss))

	if s == '':
		s = str(default)

	return s

def conv_str(c, default = ''):
	s = strconvertor(str(c))
	if s == '':
		s = default

	return "\'" + s.strip() + "\'"

def conv_int_bin(c, default = 0):
	s = ''

	ss = str(c).lstrip().rstrip()
	if ss != '':
		s = str(int(float(ss)))

	if s == '':
		s = str(default)

	return int(s)

def conv_flo_bin(c, default = 0.0):
	s = ''

	ss = str(c).lstrip().rstrip()
	if ss != '':
		s = str(float(ss))

	if s == '':
		s = str(default)

	return float(s)

def conv_str_bin(c, default = ''):
	s = strconvertor(str(c))
	if s == '':
		s = default

	return s.strip()

def conv_time(c, default = 0):
	s = ''

	_tuple = xlrd.xldate_as_tuple(c, 0)

	return conv_int(_tuple[3] * 60 * 60 + _tuple[4] * 60 + _tuple[5], default)

def conv_str_to_time(c, default = 0):
	s = ''

	sas = str_split(str(c), (':', '：'))
	if len(sas) == 2:
		return conv_int(int(sas[0]) * 60 * 60 + int(sas[1]) * 60, default);
	elif len(sas) == 3:
		return conv_int(int(sas[0]) * 60 * 60 + int(sas[1]) * 60 + int(sas[2]), default);
	elif len(sas) == 4:
		return conv_int(int(sas[0]) * 60 * 60 * 24 + int(sas[1]) *  60 * 60 + int(sas[2]) * 60 + int(sas[3]), default);
	else:
		print("error format")

	return default


# 此函数已经包含了时区
def conv_str_to_date(c, delimiters = ('-', '-')):
	s = ''
	sas = str_split(str(c), delimiters)
	if len(sas) == 3:
		return conv_int(time.mktime(datetime.datetime(int(sas[0]), int(sas[1]), int(sas[2])).timetuple()))
		'''
		ts = time.mktime(datetime.datetime(int(sas[0]), int(sas[1]), int(sas[2])).timetuple())
		uts = datetime.datetime.utcfromtimestamp(ts)

		return conv_int(time.mktime(uts.timetuple()))
		'''
	else:
		print("error format")

	return 0

# 转换时间格式 2019-01-29 18:00:00
def conv_data_and_time(c):
	s = strconvertor(str(c))
	ss = str_split(s, (' '))
	day = conv_str_to_date(ss[0])
	time = conv_str_to_time(ss[1])
	sum = int(day) + int(time)
	return str(sum)

def get_tag(sheet, sidx, tag):
	for c in range(0, len(sheet)):
		if str(sheet[c][sidx]) == tag:
			return c;

	return -1;

class RowIndex:
	def __init__(self, max_row):
		self._idx = 0
		self._max = max_row

	def Next(self):
		idx = self._idx

		self._idx += 1
		if self._idx > self._max:
			self._idx = self._max

		return idx

	def Prev(self):
		idx = self._idx
		self._idx -= 1
		if self._idx < 0:
			self._idx = 0

		return idx

	def Skip(self, count):
		idx = self._idx

		self._idx += count
		if self._idx > self._max:
			self._idx = self._max

		return idx

	def Goto(self, line):
		self._idx = line 
		if self._idx > self._max:
			self._idx = self._max

		return self._idx

	def Curr(self):
		return self._idx
