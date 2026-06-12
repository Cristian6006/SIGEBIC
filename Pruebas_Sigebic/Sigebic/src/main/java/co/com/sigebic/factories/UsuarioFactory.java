package co.com.sigebic.factories;

import java.util.Locale;
import java.util.UUID;

import co.com.sigebic.models.Usuario;
import net.datafaker.Faker;

public class UsuarioFactory {
    private static final Faker faker = new Faker(new Locale("es"));

    public static Usuario randomUser() {
        return Usuario.builder()
                .nombre(faker.name().firstName())
                .apellido(faker.name().lastName())
                .email(faker.internet().emailAddress())
                .contraseña(UUID.randomUUID().toString().substring(0, 10))
                .telefono(UUID.randomUUID().toString().substring(0, 9))
                .documento(UUID.randomUUID().toString().substring(0, 8))
                .build();
    }


}
