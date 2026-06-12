package co.com.sigebic.userInterfaces;

import net.serenitybdd.screenplay.targets.Target;
import org.openqa.selenium.By;

public class LoginUI {
    public static final Target INPUT_EMAIL = Target.the("Campo de correo").locatedBy("//input[@type='email']");
    public static final Target INPUT_PASSWORD = Target.the("Campo de contraseña").locatedBy("//input[@type='password']");
    public static final Target BOTON_ACCEDER = Target.the("Botón para acceder").locatedBy("//button[@type='submit']");
}
