CREATE OR REPLACE PROCEDURE AddCustomer(
    p_Customer_Name IN NVARCHAR2,
    p_Customer_Address IN NVARCHAR2,
    p_Customer_Contact IN NVARCHAR2,
    p_Customer_Id OUT NUMBER
) AS
BEGIN
    BEGIN
    INSERT INTO SYS.Dim_Customer (Customer_Name, Customer_Address, Customer_Contact)
    VALUES (p_Customer_Name, p_Customer_Address, p_Customer_Contact)
    RETURNING Customer_Id INTO p_Customer_Id;
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            ROLLBACK;
            -- Прокидываем исключение дальше
            RAISE;
    END;
END AddCustomer;

CREATE OR REPLACE PROCEDURE AddCustomerToUserRole(
    p_User_Role INTEGER,
    p_User_MyId INTEGER,
    p_User_Login NVARCHAR2,
    p_User_Password NVARCHAR2
)
AS
    v_Hash RAW(256); -- Размер хеша (256 бит)
BEGIN
BEGIN
    -- Хеширование пароля с использованием SHA-256
    v_Hash := DBMS_CRYPTO.HASH(Utl_I18N.STRING_TO_RAW(p_User_Password, 'AL32UTF8'), DBMS_CRYPTO.HASH_SH256);

    -- Вставка хешированного пароля в базу данных
    INSERT INTO SYS.User_Role (User_Role, User_MyId, User_Login, User_Password)
    VALUES (p_User_Role, p_User_MyId, p_User_Login, v_Hash);

    COMMIT; -- Фиксация изменений
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            ROLLBACK;
            -- Прокидываем исключение дальше
            RAISE;
    END;
END AddCustomerToUserRole;

CREATE OR REPLACE PROCEDURE CheckUserLoginExists(
    p_User_Login IN NVARCHAR2,
    p_UserExists OUT INTEGER
)
AS
BEGIN
BEGIN
    SELECT COUNT(*) INTO p_UserExists
    FROM Sys.User_Role
    WHERE User_Login = p_User_Login;
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            ROLLBACK;
            -- Прокидываем исключение дальше
            RAISE;
    END;
END CheckUserLoginExists;

CREATE OR REPLACE PROCEDURE AddSupplier(
    p_Supplier_Name IN NVARCHAR2,
    p_Supplier_Address IN NVARCHAR2,
    p_Supplier_Contact IN NVARCHAR2,
    p_Supplier_Id OUT NUMBER
)
AS
BEGIN
BEGIN
    INSERT INTO Sys.Dim_Supplier (Supplier_Name, Supplier_Address, Supplier_Contact)
    VALUES (p_Supplier_Name, p_Supplier_Address, p_Supplier_Contact)
    RETURNING Supplier_Id INTO p_Supplier_Id;

    COMMIT; -- Фиксация изменений
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            ROLLBACK;
            -- Прокидываем исключение дальше
            RAISE;
    END;
END AddSupplier;

-------------------------------------
-----        StoreHouse         -----
-------------------------------------

CREATE OR REPLACE PROCEDURE AddStoreHouseIfNotExists(
    p_StoreHouse_Address NVARCHAR2,
    p_StoreHouse_Number INTEGER,
    p_StoreHouse_Storage INTEGER,
    p_StoreHouse_Temperature INTEGER,
    p_Result OUT INTEGER
)
AS
BEGIN
    -- Проверяем, есть ли склад с указанным номером
    SELECT StoreHouse_Id
    INTO p_Result
    FROM Sys.Dim_StoreHouse
    WHERE StoreHouse_Number = p_StoreHouse_Number;

    -- Если склад существует, возвращаем -1
    IF p_Result IS NOT NULL THEN
        p_Result := -1;
    ELSE
        -- Вставляем новый склад
        INSERT INTO Sys.Dim_StoreHouse (StoreHouse_Address, StoreHouse_Number, StoreHouse_Storage, StoreHouse_Temperature)
        VALUES (p_StoreHouse_Address, p_StoreHouse_Number, p_StoreHouse_Storage, p_StoreHouse_Temperature)
        RETURNING StoreHouse_Id INTO p_Result;

        -- Фиксация изменений
        COMMIT;
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        -- Если SELECT не вернул данных (т.е., таблица пуста), присваиваем p_Result NULL

        INSERT INTO Sys.Dim_StoreHouse (StoreHouse_Address, StoreHouse_Number, StoreHouse_Storage, StoreHouse_Temperature)
        VALUES (p_StoreHouse_Address, p_StoreHouse_Number, p_StoreHouse_Storage, p_StoreHouse_Temperature)
        RETURNING StoreHouse_Id INTO p_Result;

        p_Result := 1;
    WHEN OTHERS THEN
        -- Обработка ошибок, например, запись в лог или выброс исключения
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLCODE || ' - ' || SQLERRM);
        ROLLBACK;
        -- Прокидываем исключение дальше
        RAISE;
END AddStoreHouseIfNotExists;

--drop procedure AddStoreHouseIfNotExists;

-------------------------------------
-----         Employee          -----
-------------------------------------

CREATE OR REPLACE PROCEDURE AddEmployee(
    p_Employee_Name NVARCHAR2,
    p_Employee_Role NVARCHAR2,
    p_Employee_Contact NVARCHAR2
)
AS
BEGIN
BEGIN
    -- Вставляем нового сотрудника
    INSERT INTO Sys.Dim_Employee (Employee_Name, Employee_Role, Employee_Contact)
    VALUES (p_Employee_Name, p_Employee_Role, p_Employee_Contact);

    -- Фиксация изменений
    COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            -- Обработка ошибок, например, запись в лог или выброс исключения
            DBMS_OUTPUT.PUT_LINE('Неприятная ситуация произошла: ' || ' - ' || SQLERRM);
            ROLLBACK;
            -- Прокидываем исключение дальше
            RAISE;
    END;
END AddEmployee;

--drop procedure  ADDEMPLOYEE;







CREATE OR REPLACE PROCEDURE GetFactArivalIds(
    CursorArivalId OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN CursorArivalId FOR
    SELECT Arrival_Id
    FROM Sys.Fact_Arrival;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLCODE || ' - ' || SQLERRM);
END GetFactArivalIds;

    ----------------------------------

CREATE OR REPLACE PROCEDURE GetFactShipmentIds(
    CursorShipmentId OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN CursorShipmentId FOR
    SELECT Shipment_Id
    FROM Sys.Fact_Shipment;
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLCODE || ' - ' || SQLERRM);
END GetFactShipmentIds;

    ----------------------------

CREATE OR REPLACE PROCEDURE DeleteArrivalAndProduct(
    p_Arrival_Id INTEGER
)
AS
    v_Product_Id INTEGER;
BEGIN
    -- Получаем Product_Id из Fact_Arrival по Arrival_Id
    SELECT Product_Id
    INTO v_Product_Id
    FROM Sys.Fact_Arrival
    WHERE Arrival_Id = p_Arrival_Id;

    -- Удаляем запись из Fact_Arrival по Arrival_Id
    DELETE FROM Sys.Fact_Arrival
    WHERE Arrival_Id = p_Arrival_Id;

        -- Удаляем запись из Dim_Product по Product_Id
    DELETE FROM Sys.Dim_Product
    WHERE Product_Id = v_Product_Id;

    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Запись не найдена.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLCODE || ' - ' || SQLERRM);
        ROLLBACK;
        RAISE;
END DeleteArrivalAndProduct;



CREATE OR REPLACE PROCEDURE DeleteShipmentAndProduct(
    p_Shipment_Id INTEGER
)
AS
    v_Product_Id INTEGER;
BEGIN
    SELECT Product_Id
    INTO v_Product_Id
    FROM Sys.Fact_Shipment
    WHERE Shipment_Id = p_Shipment_Id;

    DELETE FROM Sys.Fact_Shipment
    WHERE Shipment_Id = p_Shipment_Id;

    DELETE FROM Sys.Dim_Product
    WHERE Product_Id = v_Product_Id;

    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        DBMS_OUTPUT.PUT_LINE('Запись не найдена.');
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLCODE || ' - ' || SQLERRM);
        ROLLBACK;
        RAISE;
END DeleteShipmentAndProduct;


-------------------------
-- импорт из json
------------------------

CREATE OR REPLACE PROCEDURE LoadDataFromJsonEmployee(p_JsonData CLOB) AS
BEGIN
    INSERT INTO Sys.Dim_Employee (Employee_Name, Employee_Role, Employee_Contact)
    SELECT *
    FROM JSON_TABLE(
        p_JsonData,
        '$[*]'
        COLUMNS (
            Employee_Name PATH '$.Employee_Name',
            Employee_Role PATH '$.Employee_Role',
            Employee_Contact PATH '$.Employee_Contact'
        )
    );

    COMMIT; -- Фиксация изменений
EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Ошибка: ' || SQLCODE || ' - ' || SQLERRM);
        ROLLBACK;
END LoadDataFromJsonEmployee;

