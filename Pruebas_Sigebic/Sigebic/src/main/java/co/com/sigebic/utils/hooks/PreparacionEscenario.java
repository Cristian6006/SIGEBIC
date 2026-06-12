package co.com.sigebic.utils.hooks;

import io.cucumber.java.Before;
import net.serenitybdd.screenplay.actors.OnStage;
import net.serenitybdd.screenplay.actors.OnlineCast;

import static net.serenitybdd.screenplay.actors.OnStage.theActorCalled;

public class PreparacionEscenario {
    @Before
    public void PreparacionEscenario() {
        OnStage
                .setTheStage(new OnlineCast());
        theActorCalled("Administrador");
    }
}
