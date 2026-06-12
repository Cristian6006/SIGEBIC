package co.com.sigebic.models;

public class Credenciales {
    private String usuario;
    private String contraseña;

    public Credenciales() {
    }

    public Credenciales(String usuario, String contraseña) {
        this.usuario = usuario;
        this.contraseña = contraseña;
    }

    public String getUsuario() { return usuario; }
    public void setUsuario(String usuario) { this.usuario = usuario; }

    public String getContraseña() { return contraseña; }
    public void setContraseña(String contraseña) { this.contraseña = contraseña; }
}