insert INTO LunarCalenderContrastTable select convert(char(10),dateadd(d,number,'1956-1-1'),23) as Calender,--����
     dbo.fn_GetLunar(dateadd(d,number,'1956-1-1')) as Lunar --ũ��
 --����ũ�����ձ�
from master..spt_values 
where type='p'