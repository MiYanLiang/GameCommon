#encoding : utf-8

from e_base import *

def export_json(fn, compress, ResDict):
	f = create_fileA(fn)
	if f != None:
		id = 0
		s = '[model]\n'
		#iddict = GetResIDDict(ResType_Model)
		#for k1 in iddict.keys(): 
		dic = ResDict[ResType_Model]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1][0] + '@' + dic[k1][1] + '\n'
			id = id + 1

		#iddict = GetResIDDict(ResType_Skin)
		#for k1 in iddict.keys(): 
		dic = ResDict[ResType_Skin]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1] + '\n'
			id = id + 1

		id = 0
		s += '[map]\n'
		#iddict = GetResIDDict(ResType_Map)
		#for k1 in iddict.keys(): 
		dic = ResDict[ResType_Map]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1][0] + '@' + dic[k1][1] + '\n'
			id = id + 1

		id = 0
		s += '[effect]\n'
		#iddict = GetResIDDict(ResType_Effect)
		#for k1 in iddict.keys(): 
		dic = ResDict[ResType_Effect]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1] + '\n'
			id = id + 1

		id = 0
		s += '[attackeffect]\n'
		dic = ResDict[ResType_ATK_Effect]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1] + '\n'
			id = id + 1

		#id = 0
		#s += '[font]\n'
		#iddict = GetResIDDict(ResType_Effect)
		#for k1 in iddict.keys(): 
		#dic = GetResDict(ResType_Font)
		#for k1 in dic.keys():
			#s += str(id) + ' = ' + dic[k1] + '\n'
			#id = id + 1

		id = 0
		s += '[audio]\n'
		dic = ResDict[ResType_Sound]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1][0] + '@' + dic[k1][1] + '\n'
			id = id + 1

		id = 0
		s += '[file]\n'
		#iddict = GetResIDDict(ResType_Effect)
		#for k1 in iddict.keys(): 
		dic = ResDict[ResType_Texture]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1] + '\n'
			id = id + 1

		dic = ResDict[ResType_Mani]
		for k1 in dic.keys():
			s += str(id) + ' = ' + dic[k1] + '\n'
			id = id + 1

		s += '[directory]\n'
		s += '0 = script/\n'
		s += '1 = font/\n'
		s += '2 = ui/\n'
		s += '3 = textures/imgs/\n'
		if compress:
			s += '4 = shader/public/\n'
		else:
			s += '4 = shader/version2/\n'

		f.write(s)

		f.close()

if __name__ == '__main__':
	pass
