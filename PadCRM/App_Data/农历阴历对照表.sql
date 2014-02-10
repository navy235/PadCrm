insert INTO LunarCalenderContrastTable select convert(char(10),dateadd(d,number,'1956-1-1'),23) as Calender,--公历
     dbo.fn_GetLunar(dateadd(d,number,'1956-1-1')) as Lunar --农历
 --公历农历对照表
from master..spt_values 
where type='p'