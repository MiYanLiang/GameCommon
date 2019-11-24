#encoding : utf-8

import os
import sys
import shutil
import time

import traceback, multiprocessing, functools
from multiprocessing import Pool,cpu_count,Manager

import e_res
import e_base

#py_file

import e_create_BattleTable
import e_create_DifficultyChoose
import e_create_FetterTable
import e_create_ForcesTable
import e_create_LevelTable
import e_create_RandowTable
import e_create_RoleTable
import e_create_SoldierSkillTable
import e_create_Difficulty
import e_create_WarDrumTable
import e_create_TipsTable
import e_create_SoldierTypeTable
import e_create_NPCTable

taskList = (

	('/111.xlsx', e_create_BattleTable, '/BattleTable.json'),
	('/111.xlsx', e_create_DifficultyChoose, '/DifficultyChoose.json'),
	('/111.xlsx', e_create_FetterTable, '/FetterTable.json'),
	('/111.xlsx', e_create_ForcesTable, '/ForcesTable.json'),
	('/111.xlsx', e_create_LevelTable, '/LevelTable.json'),
	('/111.xlsx', e_create_RandowTable, '/RandowTable.json'),
	('/111.xlsx', e_create_RoleTable, '/RoleTable.json'),
	('/111.xlsx', e_create_SoldierSkillTable, '/SoldierSkillTable.json'),
	('/111.xlsx', e_create_Difficulty, '/DifficultyTable.json'),
	('/111.xlsx', e_create_WarDrumTable, '/WarDrumTable.json'),
	('/111.xlsx', e_create_TipsTable, '/TipsTable.json'),
	('/111.xlsx', e_create_SoldierTypeTable, '/SoldierType.json'),
	('/111.xlsx', e_create_NPCTable, '/NPCTable.json'),
)





def trace_unhandled_exceptions(func):
	@functools.wraps(func)
	def wrapped_func(*args, **kwargs):
		try:
			func(*args, **kwargs)
		except:
			# trace back.print_exc()
			raise Exception(traceback.format_exc())
	return wrapped_func

dependTaskList = (

)

@trace_unhandled_exceptions
def ResWork(db_path, ep_path, resList):
	worktask = [
		
	]
	for v in worktask:
		v[1].export_json(db_path + v[0], ep_path + v[2], resList)
		print(db_path + v[0])

@trace_unhandled_exceptions
def loadTb(start, end, dbpath, eppath):
	for x in range(start, end):
		taskList[x][1].export_json(dbpath + taskList[x][0], eppath + taskList[x][2])
		print(dbpath + taskList[x][0])

def export_json(db_path, ep_path, resList):
	e_base.prepair_path(ep_path)
	starttime = time.time()

	p = Pool()
	t_len = len(taskList)
	d_len = len(dependTaskList)

	def err_cb(exc_msg):
		print(exc_msg)
		p.terminate()

	for i in range(d_len):
		p.apply_async(dependTaskList[i], args=(db_path, ep_path), error_callback = err_cb)

	p.apply_async(ResWork, args=(db_path, ep_path, resList), error_callback = err_cb)

	average = 15
	start = 0
	end = 0
	while True:
		start = end
		if start == t_len:
			break

		end = start + average
		if end >= t_len:
			end = t_len
		#print(start, end)
		p.apply_async(loadTb, args=(start,end,db_path,ep_path), error_callback = err_cb)

	p.close()
	p.join()
	print("load client db time %0.2f" %(time.time() - starttime))

def verfiy_lua(ep_path):
	return True

def export_bin(db_path, ep_path):
	pass

def verfiy_bin(ep_path):
	return True

if __name__ == '__main__':
	db_path = sys.argv[1]
	ep_path = sys.argv[2]

	db_path = db_path.rstrip("\\")
	db_path = db_path.rstrip("/")

	ep_path = ep_path.rstrip("\\")
	ep_path = ep_path.rstrip("/")

	resList = Manager().list([dict(), dict(), dict(), dict(), dict(), dict(), dict(), dict(), dict()])
	print('开始导出客户端数据... ...')
	export_json(db_path, ep_path, resList)
	export_bin(db_path, ep_path)

	print('数据导出完成, 开始验证数据完整性... ...')
	if not verfiy_lua(ep_path):
		print('数据存在错误，查看日志了解详情')
	else:
		print('数据未发现异常')

	print('导出资源数据... ...')
	e_res.export_json('../rescpy_compress.ini', True, resList)
	e_res.export_json('../rescpy_uncompress.ini', False, resList)
	print('导出完成')