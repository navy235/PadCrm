--����ũ�����ں���  
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
  --ȷ��ũ���꿪ʼ      
  set @i=1900      
  --set @offset=@soldata      
  while @i<2050 and @offset>0      
  begin      
    set @ydays=348      
    set @mleapnum=0      
    select @ilunar=dataint from solardata where yearid=@i      
     
    --����ũ�����������      
    set @j=32768      
    while @j>8      
    begin      
      if @ilunar & @j >0      
        set @ydays=@ydays+1      
      set @j=@j/2      
    end      
    --����ũ�������ĸ��� 1-12 , û�򴫻� 0      
    set @mleap = @ilunar & 15      
    --����ũ�������µ����� ,���������������      
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
  --ȷ��ũ�������        
  set @year=@i      
  --ȷ��ũ���¿�ʼ      
  set @i = 1      
  select @ilunar=dataint from solardata where yearid=@year    
  --�ж��Ǹ���������      
  set @mleap = @ilunar & 15  
  set @bleap = 0     
  while @i < 13 and @offset > 0      
  begin      
    --�ж�����      
    set @mdays=0      
    if (@mleap > 0 and @i = (@mleap+1) and @bleap=0)      
    begin--������      
      set @i=@i-1      
      set @bleap=1 
      set @mleap1= @mleap              
      --����ũ�������µ�����      
      if @ilunar & 65536 > 0      
        set @mdays = 30      
      else       
        set @mdays = 29      
    end      
    else      
    --��������      
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
       
    --�������    
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
   
  --ȷ��ũ���½���        
  set @month=@i    
     
  --ȷ��ũ���ս���        
  set @day=ltrim(@offset) 
  --�������
  set @chinesenum=N'��һ�����������߰˾�ʮ'   
  while len(@year)>0
  select @outputdate=isnull(@outputdate,'')
         + substring(@chinesenum,left(@year,1)+1,1)
         , @year=stuff(@year,1,1,'')
  set @outputdate=@outputdate+N'��'
         + case @mleap1 when @month then N'��' else '' end
  if cast(@month as int)<10
    set @outputdate=@outputdate 
         + case @month when 1 then N'��'
             else substring(@chinesenum,left(@month,1)+1,1) 
           end
  else if cast(@month as int)>=10
    set @outputdate=@outputdate
         + case @month when '10' then N'ʮ' when 11 then N'ʮһ' 
           else N'ʮ��' end 
  set @outputdate=@outputdate + N'��'
  if cast(@day as int)<10
    set @outputdate=@outputdate + N'��'
         + substring(@chinesenum,left(@day,1)+1,1)
  else if @day between '10' and '19'
    set @outputdate=@outputdate
         + case @day when '10' then N'��ʮ' else N'ʮ'+
           substring(@chinesenum,right(@day,1)+1,1) end
  else if @day between '20' and '29'
    set @outputdate=@outputdate
         + case @day when '20' then N'��ʮ' else N'إ' end
         + case @day when '20' then N'' else 
           substring(@chinesenum,right(@day,1)+1,1) end
  else 
    set @outputdate=@outputdate+N'��ʮ'
  return @outputdate
end
GO 