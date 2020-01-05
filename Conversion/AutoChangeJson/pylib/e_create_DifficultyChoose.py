#encoding : utf-8

from e_base import *

def export_json(xls, fn):
    f = create_file(fn)
    if f != None:
        reader = xls_reader.XLSReader()
        cfgs = reader.GetSheetByIndex(xls, 12, 1)
        if cfgs != None:
            f.write("{\n")
            s = "\t\"DifficultyChoose\": [\n"
            for c in cfgs:
                ri = RowIndex(len(c))
                ss = "\t\t{\n"
                ss += "\t\t\t\"id\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"difficulty\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"playerHp\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"playerGold\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"playerMind\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"playerMorale\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"NPCHp\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"green1\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"blue1\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"purple1\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"orange1\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"green2\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"blue2\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"purple2\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"orange2\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"prestigeReward1\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"prestigeReward2\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"prestigeReward3\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"prestigeReward0\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"jiesuanGold\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"jiesuanHp\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"jiesuanChengfang\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"pyZhanyiAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"pyShiqiAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"pyChengfangAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"pyMinxinAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"npcZhanyiAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"npcShiqiAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"npcChengfangAdd\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"npcMinXinAdd\": \"" + conv_int(c[ri.Next()]) + "\"\n"
                ss += "\t\t},\n"
                s += ss
            s = s[:-2]
            s += "\n"
            s += "\t]\n"
            s += "}"
            f.write(s)
        else:
            print('sheed %s get failed.' % 'cfg')
        f.close()
def export_bin(xls, fn):
    pass