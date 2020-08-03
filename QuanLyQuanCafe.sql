
-- drop database QuanLyQuanCafe;
create database QuanLyQuanCafe;
go
use QuanLyQuanCafe;
go

-- cac table can tao
-- ban an
-- thuc an
-- loai thuc an
-- hoa don
-- thong tin hoa don
-- tai khoan


create table TableFood(
	id int identity primary key,
	name Nvarchar(100) not null default N'chua co ten table',
	status nvarchar(100) not null	default N'Trống' --trang thai la trong 0|| co nguoi 1
);
go
declare @i int =1;
while @i<=10
begin
	insert into TableFood(name) values(N'Bàn '+cast (@i as nvarchar(100)));/*cast de ep kieu*/
	set @i=@i+1;
end
go


create table Account(
	UserName Nvarchar(100) not null primary key,
	PassWord Nvarchar(1000) not null default 0,
	DisplayName Nvarchar(100) not null  default N'TamIT',
	Type int not null default 0 -- 1 la admin || 0 la staff
);
go

insert into Account(UserName,PassWord,DisplayName,Type)
values(N'TranTam',N'ILoveYou',N'TamIT',1);
insert into Account(UserName,PassWord,DisplayName,Type)
values(N'VoAnh',N'ILoveYouSoMuch',N'TamITPro',0);

create table FoodCategory(
	id int identity primary key,
	name Nvarchar(100) not null default N'chua co ten'
);
go
insert into FoodCategory(name)
values(N'nuoc trai cay');
insert into FoodCategory(name)
values(N'nuoc uong co duong');

create table Food(
	id int identity primary key,
	name Nvarchar(100) not null default N'chua co ten',
	idCategory int not null,
	price float not null default 0
	foreign key(idCategory) references dbo.FoodCategory(id)
);
go
insert into Food(name, idCategory,price)
values(N'nuoc ep cam',1,15000);
insert into Food(name, idCategory,price)
values(N'nuoc ep rau ma',1,20000);
insert into Food(name, idCategory,price)
values(N'cafe',2,18000);
insert into Food(name, idCategory,price)
values(N'soda',2,20000);
insert into Food(name, idCategory,price)
values(N'7Up',2,20000);

create table Bill(
	id int identity primary key,
	DateCheckIn date not null default getDate(),
	DateCheckOut date,
	idTable int not null,
	status int not null default 0 -- trang thai tra tien hay chua
	foreign key(idTable) references dbo.TableFood(id)
);
go

alter table Bill
add  disCount int default 0
go
update Bill set disCount=0
go

alter table Bill
add  totalPrice float default 0
go
update Bill set totalPrice=0
go
/*
insert into Bill(DateCheckIn,DateCheckOut,idTable,status)
values(GETDATE(),null,1,0);
go
insert into Bill(DateCheckIn,DateCheckOut,idTable,status)
values(GETDATE(),null,2,0);
go
insert into Bill(DateCheckIn,DateCheckOut,idTable,status)
values(GETDATE(),GETDATE(),2,1,0);
go
*/

create table BillInfo(
	id int identity primary key,
	idBill int not null,
	idFood int not null,
	count int not null default 0-- so luong thuc an

	foreign key(idBill) references dbo.Bill(id),
	foreign key(idFood) references dbo.Food(id)
);
go

/*
insert into BillInfo(idBill,idFood,count)
values(1,1,2);
insert into BillInfo(idBill,idFood,count)
values(1,2,1);
insert into BillInfo(idBill,idFood,count)
values(1,5,1);

insert into BillInfo(idBill,idFood,count)
values(2,1,1);
insert into BillInfo(idBill,idFood,count)
values(2,2,2);
insert into BillInfo(idBill,idFood,count)
values(2,3,1);

insert into BillInfo(idBill,idFood,count)
values(3,1,2);

go
*/


-- function
create proc GetAccountByUserName
@userName nvarchar(100)
as 
begin
	select * from Account where UserName=@userName;
end
go



create proc GetFoodList
as
begin
	select * from Food;
end
go



create proc Login
@userName nvarchar(100), @passWord nvarchar(100)
as
begin
	select * from Account where UserName=@userName and PassWord=@passWord;
end
go

create proc GetTableList
as
begin
	select * from TableFood;
end
go



select f.name,bi.count,f.price,f.price*bi.count as totalPrice from BillInfo as bi, Bill as b, Food as f
where bi.idBill=b.id and bi.idFood=f.id and b.idTable=1;
go 

create proc GetMenu
@id int
as
begin
		select f.name,bi.count,f.price,f.price*bi.count as totalPrice from BillInfo as bi, Bill as b, Food as f
		where bi.idBill=b.id and bi.idFood=f.id and b.idTable=@id and b.status=0;	--o day howkteam idTable=@id and  set status -- o day luc truoc minh la bi.idBill= @id
end
go


create proc InsertBill
@idTable int
as
begin
	insert into Bill(DateCheckIn,DateCheckOut,idTable,status,disCount)
	values(GETDATE(),null,@idTable,0,0);
end
go



create proc InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExistTableBillInfo int
	declare @foodCount int =1
	select @isExistTableBillInfo=id, @foodCount=b.count 
	from BillInfo as b 
	where idBill=@idBill and idFood=@idFood
	if(@isExistTableBillInfo>0)
		begin
			declare @newCount int = @foodCount +  @count
			if(@newCount>0)
				update BillInfo set count=@foodCount + @count where idFood=@idFood
			else
				delete BillInfo  where idBill=@idBill and idFood=@idFood
		end
	else
		begin
			insert into BillInfo(idBill,idFood,count)
			values(@idBill,@idFood,@count);
		end
end
go
-- trigger
create trigger UpdateBillInfo
on BillInfo for insert, update
as
begin
	declare @idBill int
	select @idBill=idBill from inserted -- lay ra idBill khi insert vao

	declare @idTable int
	select @idTable=idTable from Bill where id=@idBill and status=0

	declare @count int
	select @count=count(*) from BillInfo where idBill=@idBill
	if(@count>0)
		update TableFood set status=N'Có người' where id=@idTable;
	else
		update TableFood set status=N'Trống' where id=@idTable;
end
go

create trigger UpdateBill
on Bill for update
as
begin
	declare @idBill int
	select @idBill = id from inserted -- lay ra idBill khi insert vao

	declare @idTable int
	select @idTable=idTable from Bill where id=@idBill
	declare @count int =0
	select @count=Count(*) from Bill where idTable=@idTable and status=0
	if(@count=0)
		update TableFood set status=N'Trống' where id=@idTable;
end
go



-- chuyen ban
-- khi so sanh voi null luon dung is null
create proc SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSecondBill int

	declare @isFirstTableEmpty int =1
	declare @isSecondTableEmpty int =1

	select @idFirstBill=id from Bill where idTable=@idTable1 and status=0
	select @idSecondBill=id from Bill where idTable=@idTable2 and status=0
	if(@idSecondBill is null)
		begin
			insert into Bill(DateCheckIn,DateCheckOut,idTable,status)
			values(GETDATE(),null,@idTable2,0);
			select @idSecondBill=MAX(id) from Bill where idTable=@idTable2 and status=0
		end

	select @isSecondTableEmpty=count(*) from BillInfo where idBill=@idSecondBill

	if(@idFirstBill is null)
		begin
			insert into Bill(DateCheckIn,DateCheckOut,idTable,status)
			values(GETDATE(),null,@idTable1,0);
			select @idFirstBill=MAX(id) from Bill where idTable=@idTable1 and status=0
		end
	
	select @isFirstTableEmpty=count(*) from BillInfo where idBill=@idFirstBill 
	

	select id into IDBillInfoTable from BillInfo where idBill=@idSecondBill
	update BillInfo set idBill=@idSecondBill where idBill=@idFirstBill
	update BillInfo set idBill=@idFirstBill where id in(select * from IDBillInfoTable)
	drop table IDBillInfoTable
	if(@isFirstTableEmpty=0)
		update TableFood set status=N'Trống' where id=@idTable2
	if(@isSecondTableEmpty=0)
		update TableFood set status=N'Trống' where id=@idTable1
end
go

create proc GetListBillByDate
@dateCheckIn date, @dateCheckOut date
as
begin
	select f.name as[Tên bàn], b.totalPrice as[Tổng tiền],b.DateCheckIn as[Ngày vào],b.DateCheckOut as[Ngày ra],b.disCount as[Giảm giá]
	from Bill as b, TableFood as f
	where b.DateCheckIn >=@dateCheckIn and b.DateCheckOut<=@dateCheckOut and b.status=1 and f.id=b.idTable
end
go

create proc UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100),@passWord nvarchar(100), @newPassWord nvarchar(100)
as
begin
	declare @isRightPass int =0
	select @isRightPass=count(*) from Account where UserName=@userName and PassWord=@passWord
	if(@isRightPass>=1)
	begin
		if(@newPassWord=null or @newPassWord='')
		begin-- chi can doi ten hien thi
			update Account set DisplayName=@displayName where UserName=@userName
		end
		else
			update Account set DisplayName=@displayName, PassWord=@newPassWord where UserName=@userName
	end
end
go

create proc InsertFood
@name nvarchar(100), @idcategory int, @Price float
as
begin
insert into Food(name, idCategory,price)
values(@name,@idcategory,@Price);
end
go

create proc UpdateFood
@id int, @name nvarchar(100), @idcategory int, @Price float
as
begin
update Food set name=@name, idCategory=@idcategory, price=@Price where id=@id
end
go


create trigger tg_DeleteBillInfo
on BillInfo for delete
as
begin 
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo=id, @idBill=deleted.idBill from deleted

	declare @idTable int
	select @idTable=idTable from Bill where id=@idBill

	declare @count int =0
	select @count=count(*) from BillInfo as bi, Bill as b where b.id=bi.idBill and b.id=@idBill and b.status=0
	if(@count=0)
		update TableFood set status=N'Trống' where id=@idTable
end
go

create proc InsertAccount 
@name nvarchar(100) , @displayName nvarchar(100) , @type int
as
begin
	insert into Account(UserName,DisplayName,Type)
	values(@name,@displayName,@type)
end
go



create proc EditAccount
@name nvarchar(100), @displayname nvarchar(100), @type int
as
begin
	update Account set DisplayName=@displayname, Type=@type where UserName=@name;
end
go

create proc DeleteAccount
@name nvarchar(100)
as
begin
	delete Account where UserName=@name
end
go

create proc ResetPass
@name nvarchar(100)
as
begin
	update Account set PassWord=0 where UserName=@name
end
go

-- ho tro phan trang
create proc GetNumBillByDate
@dateCheckIn date, @dateCheckOut date
as
begin
	select count(*)
	from Bill as b, TableFood as f
	where b.DateCheckIn >=@dateCheckIn and b.DateCheckOut<=@dateCheckOut and b.status=1 and f.id=b.idTable
end
go

alter proc GetListBillByDateAndPage
@dateCheckIn date , @dateCheckOut date, @page int
as
begin
	declare @pageRows int =5
	declare @selectRows int =@pageRows
	declare @exceptRows int =(@page -1)*@pageRows
	;With Billshow as(select b.id, f.name as[Tên bàn], b.totalPrice as[Tổng tiền],b.DateCheckIn as[Ngày vào],b.DateCheckOut as[Ngày ra],b.disCount as[Giảm giá]
	from Bill as b, TableFood as f
	where b.DateCheckIn >=@dateCheckIn and b.DateCheckOut<=@dateCheckOut and b.status=1 and f.id=b.idTable)
	select top (@selectRows)* from Billshow where id not in(select top (@exceptRows) id from Billshow) 
end
go

-- new 6/6
create proc InsertCategory
@name nvarchar(100)
as
begin
insert into FoodCategory(name)
values(@name);
end
go

create proc UpdateFoodCategory
@id int, @name nvarchar(100)
as
begin
update FoodCategory set name=@name where id=@id
end
go

create proc InsertTable
@name nvarchar(100), @status nvarchar(100)
as
begin
insert into TableFood(name,status)
values(@name,@status);
end
go

create proc UpdateTable
@id int, @name nvarchar(100), @status nvarchar(100)
as
begin
update TableFood set name=@name, status=@status where id=@id
end
go
