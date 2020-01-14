#encoding : utf-8

from e_base import *

def export_json(xls, fn):
    f = create_file(fn)
    if f != None:
        reader = xls_reader.XLSReader()
        cfgs = reader.GetSheetByIndex(xls, 0, 1)
        if cfgs != None:
            f.write("{\n")
            s = "\t\"RoleTable\": [\n"
            for c in cfgs:
                ri = RowIndex(len(c))
                ss = "\t\t{\n"
                ss += "\t\t\t\"index\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"roleName\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"force\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"soldierKind\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"rarity\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"recruitingMoney\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"attack\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"defense\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"soldierNum\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"dodgeRate\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"critRate\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"critDamage\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"thumpRate\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"thumpDamage\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"exposeArmor\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"equipmentId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"allusionId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"equipmentSkillId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"soldierSkillId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"fetterSkillId\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"roleIntro\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"isHero\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"roleIcon\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"addHpPercent\": \"" + conv_str_bin(c[ri.Next()]) + "\"\n"
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