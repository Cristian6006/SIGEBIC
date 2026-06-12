package co.com.sigebic.task.Autenticacion;

import co.com.sigebic.models.Credenciales;
import co.com.sigebic.userInterfaces.LoginPage;
import co.com.sigebic.userInterfaces.LoginUI;
import net.serenitybdd.screenplay.Actor;
import net.serenitybdd.screenplay.Task;
import net.serenitybdd.screenplay.actions.Click;
import net.serenitybdd.screenplay.actions.Enter;
import net.serenitybdd.screenplay.actions.Open;

import java.util.List;

import static net.serenitybdd.screenplay.Tasks.instrumented;

public class IniciarSesion implements Task {
    private final List<Credenciales> credenciales;

    public IniciarSesion(List<Credenciales> credenciales) { this.credenciales = credenciales;}

    @Override
    public <T extends Actor> void performAs(T actor) {
        String user = credenciales.get(0).getUsuario();
        String pass = credenciales.get(0).getContraseña();

        actor.attemptsTo(
                Open.browserOn(new LoginPage()),
                Enter.theValue(user).into(LoginUI.INPUT_EMAIL),
                Enter.theValue(pass).into(LoginUI.INPUT_PASSWORD),
                Click.on(LoginUI.BOTON_ACCEDER)
        );
    }

    public static IniciarSesion conCredenciales(List<Credenciales> credenciales) {
        return instrumented(IniciarSesion.class, credenciales);
    }
}
