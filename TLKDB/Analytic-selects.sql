SELECT
    fa.Arrival_Id,
    t.Day,
    t.Month,
    t.Year,
    t.Month_Name,
    e.Employee_Name,
    s.StoreHouse_Address,
    s.StoreHouse_Number,
    p.Product_Price,
    p.Product_Name,
    su.Supplier_Name
FROM
    Sys.Fact_Arrival fa
JOIN
    Sys.Dim_Time t ON fa.Date_Id = t.Date_Id
JOIN
    Sys.Dim_Employee e ON fa.Employee_Id = e.Employee_Id
JOIN
    Sys.Dim_StoreHouse s ON fa.StoreHouse_Id = s.StoreHouse_Id
JOIN
    Sys.Dim_Product p ON fa.Product_Id = p.Product_Id
JOIN
    Sys.Dim_Supplier su ON fa.Supplier_Id = su.Supplier_Id;

SELECT fa.Shipment_Id, t.Day, t.Month, t.Year, t.Month_Name, e.Employee_Name, s.StoreHouse_Address, s.StoreHouse_Number, p.Product_Name, p.Product_Price, p.Product_Count, su.Customer_Name FROM Sys.Fact_Shipment fa JOIN Sys.Dim_Time t ON fa.Date_Id = t.Date_Id JOIN Sys.Dim_Employee e ON fa.Employee_Id = e.Employee_Id JOIN Sys.Dim_StoreHouse s ON fa.StoreHouse_Id = s.StoreHouse_Id JOIN Sys.Dim_Product p ON fa.Product_Id = p.Product_Id JOIN Sys.Dim_Customer su ON fa.Customer_Id = su.Customer_Id;

SELECT fa.Arrival_Id, t.Day, t.Month, t.Year, t.Month_Name, e.Employee_Name, s.StoreHouse_Address, s.StoreHouse_Number, p.Product_Name, p.Product_Price, p.Product_Count, su.Supplier_Name FROM Sys.Fact_Arrival fa JOIN Sys.Dim_Time t ON fa.Date_Id = t.Date_Id JOIN Sys.Dim_Employee e ON fa.Employee_Id = e.Employee_Id JOIN Sys.Dim_StoreHouse s ON fa.StoreHouse_Id = s.StoreHouse_Id JOIN Sys.Dim_Product p ON fa.Product_Id = p.Product_Id JOIN Sys.Dim_Supplier su ON fa.Supplier_Id = su.Supplier_Id;

select fa.Arrival_ID, p.Product_Name, p.Product_Count, p.Product_Price, (p.Product_Price * p.Product_Count) as "Sum_Price"
            from Sys.Fact_Arrival fa join Sys.Dim_Product p
            on fa.Product_ID = p.Product_ID;

select fa.Shipment_ID, p.Product_Name, p.Product_Count, p.Product_Price, (p.Product_Price * p.Product_Count) as "Sum_Price"
            from Sys.Fact_Shipment fa join Sys.Dim_Product p
            on fa.Product_ID = p.Product_ID;

-------------------------------

CREATE OR REPLACE PROCEDURE GetShipmentDetails(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    BEGIN
        OPEN p_Cursor FOR
        SELECT fa.Shipment_Id, t.Day, t.Month, t.Year, t.Month_Name, e.Employee_Name,
               s.StoreHouse_Address, s.StoreHouse_Number, p.Product_Name, p.Product_Price,
               p.Product_Count, su.Customer_Name
        FROM Sys.Fact_Shipment fa
        JOIN Sys.Dim_Time t ON fa.Date_Id = t.Date_Id
        JOIN Sys.Dim_Employee e ON fa.Employee_Id = e.Employee_Id
        JOIN Sys.Dim_StoreHouse s ON fa.StoreHouse_Id = s.StoreHouse_Id
        JOIN Sys.Dim_Product p ON fa.Product_Id = p.Product_Id
        JOIN Sys.Dim_Customer su ON fa.Customer_Id = su.Customer_Id;
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            RAISE;
    END;
END GetShipmentDetails;

-------------------------------

CREATE OR REPLACE PROCEDURE GetArrivalDetails(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    BEGIN
        OPEN p_Cursor FOR
        SELECT fa.Arrival_Id, t.Day, t.Month, t.Year, t.Month_Name, e.Employee_Name,
               s.StoreHouse_Address, s.StoreHouse_Number, p.Product_Name, p.Product_Price,
               p.Product_Count, su.Supplier_Name
        FROM Sys.Fact_Arrival fa
        JOIN Sys.Dim_Time t ON fa.Date_Id = t.Date_Id
        JOIN Sys.Dim_Employee e ON fa.Employee_Id = e.Employee_Id
        JOIN Sys.Dim_StoreHouse s ON fa.StoreHouse_Id = s.StoreHouse_Id
        JOIN Sys.Dim_Product p ON fa.Product_Id = p.Product_Id
        JOIN Sys.Dim_Supplier su ON fa.Supplier_Id = su.Supplier_Id;
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            RAISE;
    END;
END GetArrivalDetails;


-------------------------------

CREATE OR REPLACE PROCEDURE GetArrivalProductsDetails(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    BEGIN
        OPEN p_Cursor FOR
            select fa.Arrival_ID, p.Product_Name, p.Product_Count, p.Product_Price, (p.Product_Price * p.Product_Count) as "Sum_Price"
            from Sys.Fact_Arrival fa join Sys.Dim_Product p
            on fa.Product_ID = p.Product_ID;


    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            RAISE;
    END;
END GetArrivalProductsDetails;

-------------------------------

CREATE OR REPLACE PROCEDURE GetShipmentProductsDetails(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    BEGIN
        OPEN p_Cursor FOR
            select fa.Shipment_ID, p.Product_Name, p.Product_Count, p.Product_Price, (p.Product_Price * p.Product_Count) as "Sum_Price"
            from Sys.Fact_Shipment fa join Sys.Dim_Product p
            on fa.Product_ID = p.Product_ID;


    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            RAISE;
    END;
END GetShipmentProductsDetails;




select * from Sys.Dim_Employee where Employee_Id = 2;
select * from Sys.Dim_Employee where Employee_Contact = '+375441046566';
select * from dim_Employee where Employee_Name not like '%Работник%';
select * from Sys.Dim_Employee where Employee_Name = 'Работник-46566';

CREATE INDEX idx_employee_name ON Sys.Dim_Employee (Employee_Name);