package co.com.sigebic.models;

public class Usuario {
    private String nombre;
    private String apellido;
    private String email;
    private String contraseña;
    private String telefono;
    private String documento;

    private Usuario() {}

    public String getNombre() { return nombre;}
    public String getApellido() { return apellido;}
    public String getEmail() { return email;}
    public String getContraseña() { return contraseña;}
    public String getTelefono() { return apellido;}
    public String getDocumento() { return email;}
    public String getFullName() { return (nombre + " " + apellido).trim(); }

    public static Builder builder() { return new Builder();}

    public static class Builder {
        private final Usuario usuario = new Usuario();

        public Builder nombre(String nombre) {
            usuario.nombre = nombre;
            return this;
        }

        public Builder apellido(String apellido) {
            usuario.apellido = apellido;
            return this;
        }

        public Builder email(String email) {
            usuario.email = email;
            return this;
        }

        public Builder contraseña(String contraseña) {
            usuario.contraseña = contraseña;
            return this;
        }

        public Builder telefono(String telefono) {
            usuario.telefono = telefono;
            return this;
        }

        public Builder documento(String documento) {
            usuario.documento = documento;
            return this;
        }

        public Usuario build() {
            return usuario;
        }
    }
}
