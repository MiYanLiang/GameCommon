#encoding : utf-8

import os
import struct
import codecs

eVType_char		= 10
eVType_uchar	= 11
eVType_bool		= 12
eVType_short	= 13
eVType_ushort	= 14
eVType_int		= 15
eVType_uint		= 16
eVType_long		= 17
eVType_ulong	= 18
eVType_long64	= 19
eVType_ulong64	= 20
eVType_float	= 21
eVType_double	= 22
eVType_string	= 23

class bin_key:
	def __init__(self, key, key_type, value, value_type):
		self._key			= key
		self._key_type		= key_type
		self._value			= value
		self._value_type	= value_type

class bin_data_conv:
	''' 
		flag define
			10 -- 23: base type
			24: begin map
			25: begin map with name
			26: end map
			27: begin item
			28: end item
			29: key
			30: item
			31: item with name
	'''

	def __init__(self):
		self._flag = (10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31)

		self._func = {
			eVType_char		: self.conv_char,
			eVType_uchar	: self.conv_uchar,
			eVType_bool		: self.conv_bool,
			eVType_short	: self.conv_short,
			eVType_ushort	: self.conv_ushort,
			eVType_int		: self.conv_int,
			eVType_uint		: self.conv_uint,
			eVType_long		: self.conv_long,
			eVType_ulong	: self.conv_ulong,
			eVType_long64	: self.conv_long64,
			eVType_ulong64	: self.conv_ulong64,
			eVType_float	: self.conv_float,
			eVType_double	: self.conv_double,
			eVType_string	: self.conv_string
		}

	def conv_char(self, val):
		data = struct.pack('@B', self._flag[0])
		data += struct.pack('@b', val)

		return data

	def conv_uchar(self, val):
		data = struct.pack('@B', self._flag[1])
		data += struct.pack('@B', val)

		return data

	def conv_bool(self, val):
		data = struct.pack('@B', self._flag[2])
		data += struct.pack('@?', val)

		return data

	def conv_short(self, val):
		data = struct.pack('@B', self._flag[3])
		data += struct.pack('@h', val)

		return data

	def conv_ushort(self, val):
		data = struct.pack('@B', self._flag[4])
		data += struct.pack('@H', val)

		return data

	def conv_int(self, val):
		data = struct.pack('@B', self._flag[5])
		data += struct.pack('@i', val)

		return data

	def conv_uint(self, val):
		data = struct.pack('@B', self._flag[6])
		data += struct.pack('@I', val)

		return data

	def conv_long(self, val):
		data = struct.pack('@B', self._flag[7])
		data += struct.pack('@l', val)

		return data

	def conv_ulong(self, val):
		data = struct.pack('@B', self._flag[8])
		data += struct.pack('@L', val)

		return data

	def conv_long64(self, val):
		data = struct.pack('@B', self._flag[9])
		data += struct.pack('@q', val)

		return data

	def conv_ulong64(self, val):
		data = struct.pack('@B', self._flag[10])
		data += struct.pack('@Q', val)

		return data

	def conv_float(self, val):
		data = struct.pack('@B', self._flag[11])
		data += struct.pack('@f', val)

		return data

	def conv_double(self, val):
		data = struct.pack('@B', self._flag[12])
		data += struct.pack('@d', val)

		return data

	def conv_string(self, val):
		s = val.encode('utf-8') 

		data = struct.pack('@B', self._flag[13])
		data += struct.pack('@i', len(s))
		data += struct.pack('@%ds' % len(s), s)

		return data

	def start_table(self):
		data = struct.pack('@B', self._flag[14])

		return data

	def start_table_with_name(self, name):
		s = name.encode('utf-8') 

		data = struct.pack('@B', self._flag[15])
		data += struct.pack('@i', len(s))
		data += struct.pack('@%ds' % len(s), s)

		return data

	def end_table(self):
		data = struct.pack('@B', self._flag[16])

		return data

	def start_item(self):
		data = struct.pack('@B', self._flag[17])

		return data

	def end_item(self):
		data = struct.pack('@B', self._flag[18])

		return data

	def conv_key(self, key):
		data = struct.pack('@B', self._flag[19])
		data += self._func[key._key_type](key._key)
		data += self._func[key._value_type](key._value)

		return data

	def write_item(self, value, value_type):
		data = struct.pack('@B', self._flag[20])
		data += self._func[value_type](value)

		return data

	def write_item_with_name(self, name, value, value_type):
		s = name.encode('utf-8') 

		data = struct.pack('@B', self._flag[21])
		data += struct.pack('@i', len(s))
		data += struct.pack('@%ds' % len(s), s)
		data += self._func[value_type](value)

		return data

class bin_file:
	def __init__(self):
		self._conv = bin_data_conv()
		self._data = struct.pack('@i', 1001) #version

	def valid(self):
		return self._data != None

	def save(self, name):
		_file = open(name, 'wb')
		_file.write(self._data)
		_file.close()

	def write_char(self, val):
		self._data += self._conv.conv_char(val)

	def write_uchar(self, val):
		self._data += self._conv.conv_uchar(val)

	def write_bool(self, val):
		self._data += self._conv.conv_bool(val)

	def write_short(self, val):
		self._data += self._conv.conv_short(val)

	def write_ushort(self, val):
		self._data += self._conv.conv_ushort(val)

	def write_int(self, val):
		self._data += self._conv.conv_int(val)

	def write_uint(self, val):
		self._data += self._conv.conv_uint(val)

	def write_long(self, val):
		self._data += self._conv.conv_long(val)

	def write_ulong(self, val):
		self._data += self._conv.conv_ulong(val)

	def write_long64(self, val):
		self._data += self._conv.conv_long64(val)

	def write_ulong64(self, val):
		self._data += self._conv.conv_ulong64(val)

	def write_float(self, val):
		self._data += self._conv.conv_float(val)

	def write_double(self, val):
		self._data += self._conv.conv_double(val)

	def write_string(self, val):
		self._data += self._conv.conv_string(val)

	def start_table(self):
		self._data += self._conv.start_table()

	def start_table_with_name(self, name):
		self._data += self._conv.start_table_with_name(name)

	def end_table(self):
		self._data += self._conv.end_table()

	def start_item(self):
		self._data += self._conv.start_item()

	def end_item(self):
		self._data += self._conv.end_item()

	def write_key(self, key):
		self._data += self._conv.conv_key(key)

	def write_item(self, value, value_type):
		self._data += self._conv.write_item(value, value_type)

	def write_item_with_name(self, name, value, value_type):
		self._data += self._conv.write_item_with_name(name, value, value_type)

	def write_data(self, data):
		if data != None:
			self._data += data

