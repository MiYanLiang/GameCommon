#encoding : utf-8

from e_base import *

def export_json(xls, fn):
    f = create_file(fn)
    if f != None:
        reader = xls_reader.XLSReader()
        cfgs = reader.GetSheetByIndex(xls, 6, 1)
        if cfgs != None:
            f.write("{\n")
            s = "\t\"FetterTable\": [\n"
            for c in cfgs:
                ri = RowIndex(len(c))
                ss = "\t\t{\n"
                ss += "\t\t\t\"id\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"fetterName\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"rolesId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"attackPCT\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"defensePCT\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"soldierNumPCT\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"attack\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"defense\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"soldierNum\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"heroName\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"dodgeRate\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"critRate\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"critDamage\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"thumpRate\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"thumpDamage\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"exposeArmor\": \"" + conv_int(c[ri.Next()]) + "\"\n"
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