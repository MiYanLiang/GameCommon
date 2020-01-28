#encoding : utf-8

from e_base import *

def export_json(xls, fn):
    f = create_file(fn)
    if f != None:
        reader = xls_reader.XLSReader()
        cfgs = reader.GetSheetByIndex(xls, 23, 2)
        if cfgs != None:
            f.write("{\n")
            s = "\t\"EndingATable\": [\n"
            for c in cfgs:
                ri = RowIndex(len(c))
                ss = "\t\t{\n"
                ss += "\t\t\t\"id\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"ending\": \"" + conv_str_bin(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"gold\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"shiQi\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"junLiang\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"chengFang\": \"" + conv_int(c[ri.Next()]) + "\",\n"
                ss += "\t\t\t\"minXin\": \"" + conv_int(c[ri.Next()]) + "\"\n"
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