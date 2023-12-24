CREATE TABLE Dim_Customer (
    Customer_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Customer_Name NVARCHAR2(50) NOT NULL,
    Customer_Address NVARCHAR2(50) NOT NULL,
    Customer_Contact NVARCHAR2(50) NOT NULL
) tablespace TlkZykovDB_tablespace;

--INSERT INTO Dim_Customer (Customer_Name, Customer_Address, Customer_Contact)
--VALUES ('ОАО Стройбат', '123 пушкина', '+375291749175');

    ---------------------------------------------------------------------

CREATE TABLE Dim_Employee (
    Employee_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Employee_Name NVARCHAR2(50) NOT NULL,
    Employee_Role NVARCHAR2(50) NOT NULL,
    Employee_Contact NVARCHAR2(50) NOT NULL
) tablespace TlkZykovDB_tablespace;

-- Заполнение таблицы Dim_Employee

--INSERT INTO Dim_Employee (Employee_Name, Employee_Role, Employee_Contact)
--VALUES ('Alice Johnson', 1, '555-1111');

GRANT INSERT, SELECT ON Dim_Employee TO TLKAdmin;

    ---------------------------------------------------------------------

CREATE TABLE Dim_Time (
    Date_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Day INTEGER NOT NULL,
    Month INTEGER NOT NULL,
    Year INTEGER NOT NULL,
    Month_Name NVARCHAR2(50) NOT NULL
) tablespace TlkZykovDB_tablespace;

-- Заполнение таблицы Dim_Time

--INSERT INTO Dim_Time (Day, Month, Year, Month_Name)
--VALUES (1, 1, 2023, 'January');

    ---------------------------------------------------------------------

CREATE TABLE Dim_StoreHouse (
    StoreHouse_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    StoreHouse_Address NVARCHAR2(50) NOT NULL,
    StoreHouse_Number INTEGER NOT NULL,
    StoreHouse_Storage INTEGER NOT NULL,
    StoreHouse_Temperature INTEGER NOT NULL
) tablespace TlkZykovDB_tablespace;

    ---------------------------------------------------------------------

-- Создание таблицы Dim_Product
CREATE TABLE Dim_Product (
    Product_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Product_Name NVARCHAR2(50) NOT NULL,
    Product_Category NVARCHAR2(50) NOT NULL,
    Product_Price NUMBER NOT NULL, --------------------------------------------------------------------------
    Product_Count INTEGER NOT NULL
) tablespace TlkZykovDB_tablespace;

-- Заполнение таблицы Dim_Product

--INSERT INTO Dim_Product (Product_Name, Product_Category, Product_Price)
--VALUES ('Цемент', 'Electronics', 1200.50, 12);

    ---------------------------------------------------------------------

-- Создание таблицы Dim_Supplier
CREATE TABLE Dim_Supplier (
    Supplier_Id NUMBER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Supplier_Name NVARCHAR2(50) NOT NULL,
    Supplier_Address NVARCHAR2(50) NOT NULL,
    Supplier_Contact NVARCHAR2(50) NOT NULL
) tablespace TlkZykovDB_tablespace;

-- Заполнение таблицы Dim_Supplier
--INSERT INTO Dim_Supplier (Supplier_Name, Supplier_Address, Supplier_Contact)
--VALUES ('ABC Electronics', '123 Supplier St', '555-1111');

    ---------------------------------------------------------------------

CREATE TABLE Fact_Shipment (
    Shipment_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Date_Id INTEGER NOT NULL,
    Employee_Id INTEGER NOT NULL,
    StoreHouse_Id INTEGER NOT NULL,
    Product_Id INTEGER NOT NULL,
    Customer_Id INTEGER NOT NULL,
    CONSTRAINT fk_date FOREIGN KEY (Date_Id) REFERENCES Dim_Time (Date_Id),
    CONSTRAINT fk_employee FOREIGN KEY (Employee_Id) REFERENCES Dim_Employee (Employee_Id),
    CONSTRAINT fk_customer FOREIGN KEY (Customer_Id) REFERENCES Dim_Customer (Customer_Id),
    CONSTRAINT fk_storehouse FOREIGN KEY (StoreHouse_Id) REFERENCES Dim_StoreHouse (StoreHouse_Id),
    CONSTRAINT fk_product FOREIGN KEY (Product_Id) REFERENCES Dim_Product (Product_Id)
) tablespace TlkZykovDB_tablespace;

-- Пример заполнения таблицы Fact_Shipment
--INSERT INTO Fact_Shipment (Date_Id, Employee_Id,StoreHouse_Id, Product_Id, Customer_Id)
--VALUES (1, 1, 1, 1, 1);

    ---------------------------------------------------------------------

-- Создание таблицы Fact_Arrival
CREATE TABLE Fact_Arrival (
    Arrival_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    Date_Id INTEGER NOT NULL,
    Employee_Id INTEGER NOT NULL,
    StoreHouse_Id INTEGER NOT NULL,
    Product_Id INTEGER NOT NULL,
    Supplier_Id INTEGER NOT NULL,
    CONSTRAINT fk_arrival_employee FOREIGN KEY (Employee_Id) REFERENCES Dim_Employee (Employee_Id),
    CONSTRAINT fk_arrival_date FOREIGN KEY (Date_Id) REFERENCES Dim_Time (Date_Id),
    CONSTRAINT fk_arrival_supplier FOREIGN KEY (Supplier_Id) REFERENCES Dim_Supplier (Supplier_Id),
    CONSTRAINT fk_arrival_product FOREIGN KEY (Product_Id) REFERENCES Dim_Product (Product_Id),
    CONSTRAINT fk_arrival_storehouse FOREIGN KEY (StoreHouse_Id) REFERENCES Dim_StoreHouse (StoreHouse_Id)
) tablespace TlkZykovDB_tablespace;

-- Пример заполнения таблицы Fact_Arrival
--INSERT INTO Fact_Arrival (Date_Id, Employee_Id,StoreHouse_Id, Product_Id, Supplier_Id)
--VALUES (1, 1, 1, 1, 1);

    ---------------------------------------------------------------------
-- generated always as identity (start with 1 increment by 1)
CREATE TABLE User_Role (
    User_Id INTEGER generated always as identity (start with 1 increment by 1) PRIMARY KEY,
    User_Role INTEGER NOT NULL,
    User_MyId INTEGER NOT NULL,
    User_Login NVARCHAR2(50) NOT NULL,
    User_Password NVARCHAR2(64) NOT NULL
) tablespace TlkZykovDB_tablespace;

CREATE INDEX idx_employee_name ON Sys.Dim_Employee (Employee_Name);


-- 1 - admin
-- 2 - analytic
-- 4 - customer
-- 5 - supplier

SELECT table_name, tablespace_name
FROM all_tables
WHERE table_name = 'USER_ROLE';

DROP TABLE Fact_Shipment;
DROP TABLE Fact_Arrival;
DROP TABLE Dim_Customer;
DROP TABLE Dim_Employee;
DROP TABLE Dim_Time;
DROP TABLE Dim_StoreHouse;
DROP TABLE Dim_Product;
DROP TABLE Dim_Supplier;
DROP TABLE User_Role;

select * from Fact_Arrival;
select * from Dim_Product;
select * from Fact_Shipment;
select * from Dim_Customer;
select * from Dim_Supplier;
select * from Dim_Employee;
select * from Dim_StoreHouse;
select * from Dim_Product;
select * from Dim_Time;
select * from User_Role;

