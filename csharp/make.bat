csc /out:Test.exe /main:TestT /optimize *.cs
csc /out:Compare.exe /main:CompareT /optimize *.cs
csc /out:MergeAndRenameFigures.exe MergeAndRenameFigures.cs
test i:\documentos\t1test
compare i:\documentos\t1test