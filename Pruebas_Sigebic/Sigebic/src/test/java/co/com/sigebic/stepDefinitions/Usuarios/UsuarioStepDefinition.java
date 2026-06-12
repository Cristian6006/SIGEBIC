package co.com.sigebic.stepDefinitions.Usuarios;

import co.com.sigebic.task.NavegarA;
import io.cucumber.java.es.Dado;
import net.serenitybdd.screenplay.actors.OnStage;

public class UsuarioStepDefinition {
    @Dado("se encuentre en la pagina de gestion gestion de Usuarios")
    public void seEncuentreEnLaPaginaDeGestionGestionDeUsuarios() {
        OnStage.theActorInTheSpotlight().attemptsTo(
                NavegarA.laPaginaUsuarios()
        );
    }
}
