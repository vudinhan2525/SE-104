﻿USE master
IF EXISTS (SELECT * FROM SYS.DATABASES WHERE NAME = 'DaiLy')
BEGIN
	ALTER DATABASE DaiLy SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE DaiLy;
END
GO

CREATE DATABASE DaiLy
GO

USE DaiLy
GO

create table USERS
(
	Username varchar(40),
	Password varchar(40)
	)

INSERT INTO USERS(Username, Password)
Values	('U1','123');
GO

CREATE TABLE PhieuNhap(
	MaPhieuNhap int not null ,
	MaMatHang int not null,
	DonGia money,
	DonViTinh nvarchar(50),
	NgayLapPhieu datetime,
	SoLuong int,
	ThanhTien money
)

CREATE TABLE MatHang(
	MaMatHang int PRIMARY KEY ,
	TenMatHang nvarchar(100),
	Gia decimal,
	SoLuong int,
	DonViTinh nvarchar(50)
)

CREATE TABLE PhieuXuat(
	MaPhieuXuat int IDENTITY(1,1),
	MaMatHang int not null,
	MaDaiLy int,
	NgayLapPhieu datetime,
	DonViTinh nvarchar(50),
	SoLuong int,
	DonGia decimal,
	ThanhTien decimal
)

CREATE TABLE DaiLy(
	MaDaiLy int IDENTITY(1,1) PRIMARY KEY,
	TenDaiLy nvarchar(255),
	SoDienThoai nvarchar(15),
	Quan nvarchar(50),
	Avatar nvarchar(100),
	DiaChi nvarchar(50),
	Loai tinyint,
	NgayTiepNhan datetime,
	KhoanNo money,
	Email nvarchar(255)
)

CREATE TABLE PhieuThu(
	MaPhieuThu int IDENTITY(1,1) PRIMARY KEY,
	MaDaiLy int NOT NULL,
	TenDaiLy nvarchar(200),
	Avatar nvarchar(1000),
	DiaChi nvarchar(300),
	SoDienThoai nvarchar(50),
	Email nvarchar(100),
	SoTienThu money,
	NgayThu datetime
)

ALTER TABLE PhieuNhap
ADD CONSTRAINT PK_PhieuNhap PRIMARY KEY (MaPhieuNhap, MaMatHang)

ALTER TABLE PhieuNhap
ADD CONSTRAINT FK_PhieuNhap_MatHang
FOREIGN KEY(MaMatHang) REFERENCES MatHang(MaMatHang)

ALTER TABLE PhieuXuat
ADD CONSTRAINT PK_PhieuXuat PRIMARY KEY(MaPhieuXuat, MaMatHang)

ALTER TABLE PhieuXuat
ADD CONSTRAINT FK_PhieuXuat_MatHang
FOREIGN KEY(MaMatHang) REFERENCES MatHang(MaMatHang)

ALTER TABLE PhieuXuat
ADD CONSTRAINT FK_PhieuXuat_DaiLy
FOREIGN KEY(MaDaiLy) REFERENCES DaiLy(MaDaiLy)

ALTER TABLE PhieuThu
ADD CONSTRAINT FK_DaiLy_PhieuThu
FOREIGN KEY(MaDaiLy) REFERENCES DaiLy(MaDaiLy)
GO


INSERT INTO MatHang( MaMatHang, TenMatHang, Gia, SoLuong, DonViTinh)
Values ('1','GTX-3060',650000,'12','Don vi'),
		('2','Razor Viper',160000,'24','Don vi');
INSERT INTO MatHang( MaMatHang, TenMatHang, Gia, SoLuong, DonViTinh)
Values ('3',N'Dây chuyền đá quý',1000000,'26','Don vi')



INSERT INTO DaiLy(TenDaiLy, SoDienThoai, Quan, Avatar, DiaChi, Loai, NgayTiepNhan, KhoanNo, Email)
Values ( 'ShopDunk Official Store','123456789',N'Quận 1','https://mms.img.susercontent.com/vn-11134216-7r98o-llx8zp2z3dnj5b',N'Hồ Chí Minh',1,'2024-02-03',0,'shopdunk2@gmail.com'),
		( 'Coolmate Store','123456789',N'Quận 10','https://mms.img.susercontent.com/vn-11134216-7r98o-lqmqpjrwbftebc_tn',N'Hồ Chí Minh',1,'2024-05-10',0,'coolmate12@gmail.com'),
		( 'GEAR VN STORE','123456789',N'Quận 7','https://datviettour.com.vn/uploads/images/khach-hang/Gearn/logo-gearvn.jpg',N'Hồ Chí Minh',1,'2024-05-16',0,'gearvn@gmail.com')
SELECT * FROM MatHang
GO

CREATE TRIGGER UpdateGoodsQuantity 
ON PhieuNhap
AFTER INSERT 
As	
BEGIN
    UPDATE MatHang
    SET SoLuong = SoLuong + i.TongNhap
	From MatHang
	Inner Join (
    SELECT MaMatHang, SUM(SoLuong) AS TongNhap
        FROM inserted
        GROUP BY MaMatHang
    ) i ON MatHang.MaMatHang = i.MaMatHang;
	UPDATE PhieuNhap
    SET DonViTinh = mh.DonViTinh
    FROM PhieuNhap pn
    JOIN MatHang mh ON pn.MaMatHang = mh.MaMatHang
	WHERE pn.MaPhieuNhap IN (SELECT MaPhieuNhap FROM inserted);
END;
GO
CREATE TRIGGER UpdateGoodsQuantity2 
ON PhieuXuat
AFTER INSERT 
As	
BEGIN
    UPDATE MatHang
    SET SoLuong = SoLuong- i.TongNhap
	From MatHang
	Inner Join (
    SELECT MaMatHang, SUM(SoLuong) AS TongNhap
        FROM inserted
        GROUP BY MaMatHang
    ) i ON MatHang.MaMatHang = i.MaMatHang;
	
END;
GO
