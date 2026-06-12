package co.com.sigebic.stepDefinitions.Autenticacion;

import co.com.sigebic.models.Credenciales;
import co.com.sigebic.questions.Autenticacion.TituloDashboard;
import co.com.sigebic.task.Autenticacion.IniciarSesion;
import io.cucumber.java.es.Cuando;
import io.cucumber.java.es.Entonces;
import net.serenitybdd.screenplay.actors.OnStage;
import java.util.List;
import static net.serenitybdd.screenplay.GivenWhenThen.seeThat;

public class AutenticacionStepDefinition {
    @Cuando("el administrador inicie sesion con las credenciales correctas")
    public void inicieSesionConLasCredencialesCorrectas(List<Credenciales> credenciales) {
        OnStage.theActorInTheSpotlight().attemptsTo(
                IniciarSesion.conCredenciales(credenciales)
        );
    }
    @Entonces("deberia ser redirigido a la pagina principal")
    public void deberiaSerRedirigidoALaPaginaPrincipal() {
        OnStage.theActorInTheSpotlight().should(
                seeThat(TituloDashboard.tituloDashboard())
        );
    }
}
