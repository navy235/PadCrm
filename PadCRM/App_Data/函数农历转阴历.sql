--��������ũ����ȡ�������ں���  
if object_id('fn_GetDate') is not null
    drop function fn_GetDate
go 
create function dbo.fn_GetDate(@day nvarchar(50))      
returns datetime
as      
begin
    return(select Calender from LunarCalenderContrastTable where Lunar=@day)
 --return(select ���� from ����ũ�����ձ� where ũ��=@day)
    
--return(select yearid from SolarData  where dbo.fn_GetLunar(convert(varchar(10),getdate(),23))=@day)
end
go