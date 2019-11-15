#encoding : utf-8

import xlrd
import struct

class XLSReader:
	def __init__(self):
		pass

	def GetSheetByName(self, name, sheet, row = 2):
		bk = xlrd.open_workbook(name)
		try:
			sh = bk.sheet_by_name(sheet)
		except:
			print("no sheet in %s named %s" % name, sheet)

			return None

		nrows = sh.nrows
		ncols = sh.ncols

		row_list = []
		for i in range(row, nrows):
			row_data = sh.row_values(i)
			row_list.append(row_data)

		return row_list

	# arg1 self
	# arg2 excel name
	# arg3 index of the excel
	# arg4 the row which start to read, default row 2
	def GetSheetByIndex(self, name, idx, row = 2):
		bk = xlrd.open_workbook(name)
		try:
			sh = bk.sheet_by_index(idx)
		except:
			print("no sheet in %s index %d" % name, idx)

			return None

		nrows = sh.nrows
		ncols = sh.ncols

		row_list = []
		for i in range(row, nrows):
			row_data = sh.row_values(i)
			row_list.append(row_data)

		return row_list
