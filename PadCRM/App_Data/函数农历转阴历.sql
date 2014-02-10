--创建根据农历获取公历日期函数  
if object_id('fn_GetDate') is not null
    drop function fn_GetDate
go 
create function dbo.fn_GetDate(@day nvarchar(50))      
returns datetime
as      
begin
    return(select Calender from LunarCalenderContrastTable where Lunar=@day)
 --return(select 公历 from 公历农历对照表 where 农历=@day)
    
--return(select yearid from SolarData  where dbo.fn_GetLunar(convert(varchar(10),getdate(),23))=@day)
end
go