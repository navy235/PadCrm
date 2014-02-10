--创建农历日期函数  
if object_id('fn_GetLunar') is not null
    drop function fn_GetLunar
go 
create function dbo.fn_GetLunar(@solarday datetime)      
returns nvarchar(30)    
as      
begin      
  declare @soldata int      
  declare @offset int      
  declare @ilunar int      
  declare @i int       
  declare @j int       
  declare @ydays int      
  declare @mdays int      
  declare @mleap int  
  declare @mleap1 int    
  declare @mleapnum int      
  declare @bleap smallint      
  declare @temp int      
  declare @year nvarchar(10)       
  declare @month nvarchar(10)      
  declare @day nvarchar(10)  
  declare @chinesenum nvarchar(10)         
  declare @outputdate nvarchar(30)       
  set @offset=datediff(day,'1900-01-30',@solarday)      
  --确定农历年开始      
  set @i=1900      
  --set @offset=@soldata      
  while @i<2050 and @offset>0      
  begin      
    set @ydays=348      
    set @mleapnum=0      
    select @ilunar=dataint from solardata where yearid=@i      
     
    --传回农历年的总天数      
    set @j=32768      
    while @j>8      
    begin      
      if @ilunar & @j >0      
        set @ydays=@ydays+1      
      set @j=@j/2      
    end      
    --传回农历年闰哪个月 1-12 , 没闰传回 0      
    set @mleap = @ilunar & 15      
    --传回农历年闰月的天数 ,加在年的总天数上      
    if @mleap > 0      
    begin      
      if @ilunar & 65536 > 0      
        set @mleapnum=30      
      else       
        set @mleapnum=29           
      set @ydays=@ydays+@mleapnum      
    end      
    set @offset=@offset-@ydays      
    set @i=@i+1      
  end      
  if @offset <= 0      
  begin      
    set @offset=@offset+@ydays      
    set @i=@i-1      
  end      
  --确定农历年结束        
  set @year=@i      
  --确定农历月开始      
  set @i = 1      
  select @ilunar=dataint from solardata where yearid=@year    
  --判断那个月是润月      
  set @mleap = @ilunar & 15  
  set @bleap = 0     
  while @i < 13 and @offset > 0      
  begin      
    --判断润月      
    set @mdays=0      
    if (@mleap > 0 and @i = (@mleap+1) and @bleap=0)      
    begin--是润月      
      set @i=@i-1      
      set @bleap=1 
      set @mleap1= @mleap              
      --传回农历年闰月的天数      
      if @ilunar & 65536 > 0      
        set @mdays = 30      
      else       
        set @mdays = 29      
    end      
    else      
    --不是润月      
    begin      
      set @j=1      
      set @temp = 65536       
      while @j<=@i      
      begin      
        set @temp=@temp/2      
        set @j=@j+1      
      end      
     
      if @ilunar & @temp > 0      
        set @mdays = 30      
      else      
        set @mdays = 29      
    end      
       
    --解除润月    
    if @bleap=1 and @i= (@mleap+1)    
      set @bleap=0    
   
    set @offset=@offset-@mdays      
    set @i=@i+1      
  end      
     
  if @offset <= 0      
  begin      
    set @offset=@offset+@mdays      
    set @i=@i-1      
  end      
   
  --确定农历月结束        
  set @month=@i    
     
  --确定农历日结束        
  set @day=ltrim(@offset) 
  --输出日期
  set @chinesenum=N'〇一二三四五六七八九十'   
  while len(@year)>0
  select @outputdate=isnull(@outputdate,'')
         + substring(@chinesenum,left(@year,1)+1,1)
         , @year=stuff(@year,1,1,'')
  set @outputdate=@outputdate+N'年'
         + case @mleap1 when @month then N'润' else '' end
  if cast(@month as int)<10
    set @outputdate=@outputdate 
         + case @month when 1 then N'正'
             else substring(@chinesenum,left(@month,1)+1,1) 
           end
  else if cast(@month as int)>=10
    set @outputdate=@outputdate
         + case @month when '10' then N'十' when 11 then N'十一' 
           else N'十二' end 
  set @outputdate=@outputdate + N'月'
  if cast(@day as int)<10
    set @outputdate=@outputdate + N'初'
         + substring(@chinesenum,left(@day,1)+1,1)
  else if @day between '10' and '19'
    set @outputdate=@outputdate
         + case @day when '10' then N'初十' else N'十'+
           substring(@chinesenum,right(@day,1)+1,1) end
  else if @day between '20' and '29'
    set @outputdate=@outputdate
         + case @day when '20' then N'二十' else N'廿' end
         + case @day when '20' then N'' else 
           substring(@chinesenum,right(@day,1)+1,1) end
  else 
    set @outputdate=@outputdate+N'三十'
  return @outputdate
end
GO 