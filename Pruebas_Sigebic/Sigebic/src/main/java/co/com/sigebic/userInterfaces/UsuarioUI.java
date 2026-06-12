package co.com.sigebic.userInterfaces;

import net.serenitybdd.screenplay.targets.Target;
import org.openqa.selenium.By;

public class UsuarioUI {
    // COMUNES
    public static final Target USUARIO_EXISTE = Target.the("Usuario existente en la lista").locatedBy("//div[@role='row'][.//div[text()='{0}']]");
    public static final Target INPUT_PRIMER_NOMBRE = Target.the("Campo Primer Nombre").located(By.name("firstName"));
    public static final Target INPUT_SEGUNDO_NOMBRE = Target.the("Campo de segundo nombre").located(By.name("middleName"));
    public static final Target INPUT_APELLIDO = Target.the("Campo de apellido").located(By.name("lastName"));
    public static final Target INPUT_ID = Target.the("Campo de ID").locatedBy("//label[normalize-space()='Employee Id']/ancestor::div[contains(@class,'oxd-input-group')]" + "//input");
    public static final Target BOTON_SALVAR_USUARIOS = Target.the("Bonton de guardado de usuarios").locatedBy("//button[contains(., ' Save ')]");

    // AGREGAR USUARIO
    public static final Target BOTON_AGREGAR_USUARIOS = Target.the("Bonton Agregar Usuarios").locatedBy("//button[contains(., 'Nuevo usuario')]");
    public static final Target USUARIO_VISIBLE = Target.the("Usuario").locatedBy("//h6[normalize-space(.)='{0}']");

    //CONSULTAR USUARIOS
    public static final Target EMPLOYEE_NAME = Target.the("Campo Employee Name").located(By.xpath("//label[normalize-space()='Employee Name']" + "/following::input[@placeholder='Type for hints...'][1]"));
    public static final Target BOTON_BUSCAR = Target.the("Boton Buscar").locatedBy("//button[contains(@class, 'oxd-button') and normalize-space()='Search']");

    //BORRAR USUARIO
    public static final Target BOTON_ACTUALIZAR_USUARIO = Target.the("Boton de refresco de usuarios").locatedBy("//li[contains(., 'Employee List')]");
    public static final Target BOTON_BORRAR_USUARIO = Target.the("Boton de borrado de usuarios").locatedBy("//div[@role='row'][.//div[text()='{0}']]//i[contains(@class,'bi-trash')]/parent::button");
    public static final Target BOTON_CONFIRMAR_BORRADO = Target.the("Boton de confirmado de dorrado").locatedBy("//button[normalize-space()='Yes, Delete']");

}
