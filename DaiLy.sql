USE master
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
	MaPhieuNhap int not null,
	MaMatHang int not null,
	DonGia money,
	DonViTinh varchar(50),
	NgayLapPhieu datetime,
	SoLuong int,
	ThanhTien money
)

CREATE TABLE MatHang(
	MaMatHang int primary key,
	TenMatHang varchar(50),
	Gia money,
	SoLuong int,
	DonViTinh varchar(50)
)

CREATE TABLE PhieuXuat(
	MaPhieuXuat int not null,
	MaMatHang int not null,
	MaDaiLy int,
	NgayLapPhieu datetime,
	DonViTinh varchar(50),
	SoLuong int,
	DonGia money,
	ThanhTien money
)

CREATE TABLE DaiLy(
	MaDaiLy int primary key,
	TenDaiLy varchar(255),
	SoDienThoai varchar(15),
	Quan varchar(50),
	DiaChi varchar,
	Loai tinyint,
	NgayTiepNhan datetime,
	KhoanNo money,
	Email varchar(255)
)

CREATE TABLE PhieuThu(
	MaPhieuThu int primary key,
	MaDaiLy int,
	SoTienThu money,
	NgayThu datetime
)
Go


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



INSERT INTO MatHang(MaMatHang, TenMatHang, Gia, SoLuong, DonViTinh)
Values ('001','GTX-3060',6500000,'2','Don vi');

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
    SET DonGia = mh.Gia,
        DonViTinh = mh.DonViTinh
    FROM PhieuNhap pn
    INNER JOIN MatHang mh ON pn.MaMatHang = mh.MaMatHang
    WHERE pn.MaPhieuNhap IN (SELECT MaPhieuNhap FROM inserted);
END;