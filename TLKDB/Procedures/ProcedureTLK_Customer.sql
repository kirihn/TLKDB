CREATE OR REPLACE PROCEDURE AddDimTimeCustomer(
p_Day INTEGER,
    p_Month INTEGER,
    p_Year INTEGER,
    p_Month_Name NVARCHAR2,
    p_Result OUT INTEGER
) AS
BEGIN
    -- Пытаемся найти запись с такими значениями
    BEGIN
        SELECT Date_Id INTO p_Result
        FROM Sys.Dim_Time
        WHERE Day = p_Day AND Month = p_Month AND Year = p_Year AND Month_Name = p_Month_Name;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            -- Если запись не найдена, устанавливаем p_Result в NULL
            p_Result := NULL;
    END;

    -- Если запись не найдена, вставляем новую
    IF p_Result IS NULL THEN
        INSERT INTO Sys.Dim_Time (Day, Month, Year, Month_Name)
        VALUES (p_Day, p_Month, p_Year, p_Month_Name)
        RETURNING Date_Id INTO p_Result;

        -- Фиксация изменений
        COMMIT;
    END IF;
END AddDimTimeCustomer;

--drop procedure ADDDIMTIMECUSTOMER;


----------------
---------------
----------------

CREATE OR REPLACE PROCEDURE AddDimProductCustomer(
    p_Product_Name IN NVARCHAR2,
    p_Product_Category IN NVARCHAR2,
    p_Product_Price IN NUMBER,
    p_Product_Count IN INTEGER,
    p_Product_Id OUT INTEGER
)
AS
BEGIN
    -- Вставляем новый продукт
    INSERT INTO Sys.Dim_Product (Product_Name, Product_Category, Product_Price, Product_Count)
    VALUES (p_Product_Name, p_Product_Category, p_Product_Price, p_Product_Count)
    RETURNING Product_Id INTO p_Product_Id;

    -- Фиксация изменений
    COMMIT;
END AddDimProductCustomer;


--drop procedure  AddDimProductCustomer;


-----------------------------------------------

----------------------------------------

CREATE OR REPLACE PROCEDURE GetRandomStoreHouseIdC(
    p_RandomId OUT INTEGER
)
AS
BEGIN
    SELECT StoreHouse_Id
    INTO p_RandomId
    FROM (
        SELECT StoreHouse_Id
        FROM Sys.Dim_StoreHouse
        ORDER BY DBMS_RANDOM.VALUE
    )
    WHERE ROWNUM = 1;
END GetRandomStoreHouseIdC;

----------------------------------------

CREATE OR REPLACE PROCEDURE GetRandomEmployeeIdC(
    p_RandomId OUT INTEGER
)
AS
BEGIN
    SELECT Employee_Id
    INTO p_RandomId
    FROM (
        SELECT Employee_Id
        FROM Sys.Dim_Employee
        ORDER BY DBMS_RANDOM.VALUE
    )
    WHERE ROWNUM = 1;
    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: '  SQLCODE  ' - ' || SQLERRM);
        ROLLBACK;
END GetRandomEmployeeIdC;

----------------------------------------

-----------------------------------------------
CREATE OR REPLACE PROCEDURE AddShipment(
    p_Date_Id INTEGER,
    p_Employee_Id INTEGER,
    p_StoreHouse_Id INTEGER,
    p_Product_Id INTEGER,
    p_Customer_Id INTEGER
)
AS
BEGIN
    INSERT INTO Sys.Fact_Shipment (Date_Id, Employee_Id, StoreHouse_Id, Product_Id, Customer_Id)
    VALUES (p_Date_Id, p_Employee_Id, p_StoreHouse_Id, p_Product_Id, p_Customer_Id);

    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: '  SQLCODE  ' - ' || SQLERRM);
        ROLLBACK;
END AddShipment;