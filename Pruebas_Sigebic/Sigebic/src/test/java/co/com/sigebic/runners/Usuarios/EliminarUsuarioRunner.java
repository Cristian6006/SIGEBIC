package co.com.sigebic.runners.Usuarios;

import io.cucumber.junit.CucumberOptions;
import net.serenitybdd.cucumber.CucumberWithSerenity;
import org.junit.runner.RunWith;

@RunWith(CucumberWithSerenity.class)
@CucumberOptions(
        features = "classpath:features",
        glue = "co.com.sigebic",
        tags = "@EliminarUsuario",
        snippets = CucumberOptions.SnippetType.CAMELCASE
)

public class EliminarUsuarioRunner {
}
