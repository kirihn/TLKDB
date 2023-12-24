CREATE OR REPLACE PROCEDURE CHECKUSERLOGINPASSWORD (
    p_Login NVARCHAR2,
    p_Password NVARCHAR2,
    p_UserRole OUT NUMBER,
    p_UserMyId OUT NUMBER
) AS
    v_UserRole NUMBER;
    v_UserMyId NUMBER;
    v_StoredPassword NVARCHAR2(64); -- Предположим, что пароль хранится в виде хэша
    v_InputPasswordHash RAW(64); -- Предположим, что вы используете SHA-256

BEGIN
    -- Получаем хэш пароля из базы данных
    SELECT User_Role, User_MyId, User_Password
    INTO v_UserRole, v_UserMyId, v_StoredPassword
    FROM SYS.User_Role
    WHERE User_Login = p_Login;

    -- Если пользователя не найдено, устанавливаем значения по умолчанию
    IF SQL%NOTFOUND THEN
        v_UserRole := NULL;
        v_UserMyId := NULL;
    ELSE
        -- Получаем хэш введенного пароля
        v_InputPasswordHash := DBMS_CRYPTO.HASH(Utl_I18N.STRING_TO_RAW(p_Password, 'AL32UTF8'), DBMS_CRYPTO.HASH_SH256);

        -- Проверяем, совпадает ли хэш пароля
        IF v_InputPasswordHash = v_StoredPassword THEN
            -- Если пароль совпадает, возвращаем роли и MyId
            p_UserRole := v_UserRole;
            p_UserMyId := v_UserMyId;
        ELSE
            -- Если пароль не совпадает, устанавливаем значения по умолчанию
            p_UserRole := NULL;
            p_UserMyId := NULL;
        END IF;
    END IF;
END CHECKUSERLOGINPASSWORD;



DECLARE
    v_Login NVARCHAR2(50) := 'admin'; -- Введите нужное имя пользователя
    v_Password NVARCHAR2(50) := 'admin'; -- Введите нужный пароль
    v_UserRole NUMBER;
    v_UserMyId NUMBER;
BEGIN
    CHECKUSERLOGINPASSWORD(
        p_Login => v_Login,
        p_Password => v_Password,
        p_UserRole => v_UserRole,
        p_UserMyId => v_UserMyId
    );

    -- Выводим результаты
    DBMS_OUTPUT.PUT_LINE('User Role: ' || v_UserRole);
    DBMS_OUTPUT.PUT_LINE('User MyId: ' || v_UserMyId);
END;
--DROP PROCEDURE CheckUserLoginPassword;

select * from Sys.User_Role;

SELECT User_Role, User_MyId
    FROM SYS.User_Role
    WHERE User_Login = 'Hippo' AND User_Password = '1';