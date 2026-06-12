package co.com.sigebic.questions.Autenticacion;

import net.serenitybdd.screenplay.Actor;
import net.serenitybdd.screenplay.Question;
import net.serenitybdd.screenplay.questions.Text;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import static co.com.sigebic.userInterfaces.DashBoardUI.TITULO_DASHBOARD;

public class TituloDashboard implements Question<Boolean> {
    private static final Logger logger =
            LoggerFactory.getLogger(TituloDashboard.class);

    public static final String TITULO_ESPERADO = "Dashboard";

    public static TituloDashboard tituloDashboard() { return new TituloDashboard();}

    @Override
    public Boolean answeredBy(Actor actor) {
        try {
            String texto = Text.of(TITULO_DASHBOARD).answeredBy(actor).trim();
            logger.info("Texto capturado: {}", texto);
            return texto.equalsIgnoreCase(TITULO_ESPERADO);
        } catch (Exception e) {
            logger.error("El titulo de dashboard no fue encontrado: {}", e.getMessage());
            return false;
        }
    }
}
