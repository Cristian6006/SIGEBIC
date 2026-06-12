package co.com.sigebic.runners.Autenticacion;

import io.cucumber.junit.CucumberOptions;
import net.serenitybdd.cucumber.CucumberWithSerenity;
import org.junit.runner.RunWith;

@RunWith(CucumberWithSerenity.class)
@CucumberOptions(
        features = "classpath:features",
        glue = "co.com.sigebic",
        tags = "@Autenticacion",
        snippets = CucumberOptions.SnippetType.CAMELCASE

)

public class LoginRunner {
}
