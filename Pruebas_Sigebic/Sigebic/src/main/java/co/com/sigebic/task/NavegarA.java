package co.com.sigebic.task;

import co.com.sigebic.userInterfaces.DashBoardUI;
import net.serenitybdd.screenplay.Actor;
import net.serenitybdd.screenplay.Task;
import net.serenitybdd.screenplay.actions.Click;

import static net.serenitybdd.screenplay.Tasks.instrumented;

public class NavegarA implements Task {
    @Override
    public <T extends Actor> void  performAs(T actor) {
        actor.attemptsTo(
                Click.on(DashBoardUI.BOTON_NAV_USUARIOS)
        );
    }
    public static NavegarA laPaginaUsuarios() {
        return instrumented(NavegarA.class);
    }
}

