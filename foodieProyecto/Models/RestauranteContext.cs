using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace foodieProyecto.Models;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext()
    {
    }

    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<ComboPromocion> ComboPromocions { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<LoginCliente> LoginClientes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuCombo> MenuCombos { get; set; }

    public virtual DbSet<MenuPlato> MenuPlatos { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PagoTarjetum> PagoTarjeta { get; set; }

    public virtual DbSet<PedidoLocal> PedidoLocals { get; set; }

    public virtual DbSet<Plato> Platos { get; set; }

    public virtual DbSet<PlatosCombo> PlatosCombos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-TCPUJ1K; Initial Catalog=restaurante; User ID=juan_monroy; Password=12345; TrustServerCertificate=True; Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cargo__3213E83F69C0920C");

            entity.ToTable("cargo");

            entity.HasIndex(e => e.Nombre, "UQ__cargo__72AFBCC63CEBFEF8").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__categori__3213E83F3E8D2924");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Nombre, "UQ__categori__72AFBCC6320C8DE4").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__C2FF245D4AB8BBB8");

            entity.ToTable("Cliente");

            entity.HasIndex(e => e.Loginid, "UQ__Cliente__1F5DF0A616D56A9D").IsUnique();

            entity.Property(e => e.ClienteId)
                .ValueGeneratedNever()
                .HasColumnName("clienteId");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Latitud)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("latitud");
            entity.Property(e => e.Loginid).HasColumnName("loginid");
            entity.Property(e => e.Longitud)
                .HasColumnType("decimal(10, 6)")
                .HasColumnName("longitud");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Login).WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(d => d.Loginid)
                .HasConstraintName("FK__Cliente__loginid__02084FDA");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__combos__3213E83F0C0F1CD2");

            entity.ToTable("combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Combos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__combos__categori__47DBAE45");
        });

        modelBuilder.Entity<ComboPromocion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__combo_pr__3213E83F545CF04C");

            entity.ToTable("combo_promocion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.PromocionId).HasColumnName("promocion_id");

            entity.HasOne(d => d.Combo).WithMany(p => p.ComboPromocions)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__combo_pro__combo__5165187F");

            entity.HasOne(d => d.Promocion).WithMany(p => p.ComboPromocions)
                .HasForeignKey(d => d.PromocionId)
                .HasConstraintName("FK__combo_pro__promo__52593CB8");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.DetalleFacturaId).HasName("PK__Detalle___FC2C9A8FECB03EDD");

            entity.ToTable("Detalle_Factura");

            entity.Property(e => e.DetalleFacturaId).HasColumnName("detalle_factura_id");
            entity.Property(e => e.DetallePedidoId).HasColumnName("detalle_pedido_id");
            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.DetallePedido).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.DetallePedidoId)
                .HasConstraintName("FK__Detalle_F__detal__797309D9");

            entity.HasOne(d => d.Factura).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.FacturaId)
                .HasConstraintName("FK__Detalle_F__factu__787EE5A0");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__Detalle___C08768CF315B0807");

            entity.ToTable("Detalle_Pedido");

            entity.Property(e => e.IdDetallePedido).HasColumnName("id_detalle_pedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Comentarios)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comentarios");
            entity.Property(e => e.EncabezadoId).HasColumnName("encabezado_id");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente")
                .HasColumnName("estado");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.TipoItem)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_Item");
            entity.Property(e => e.TipoVenta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_venta");

            entity.HasOne(d => d.Encabezado).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.EncabezadoId)
                .HasConstraintName("FK__Detalle_P__encab__693CA210");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__empleado__3213E83F933F4976");

            entity.ToTable("empleados");

            entity.HasIndex(e => e.Codigo, "UQ__empleado__40F9A206CF21E784").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.CargoId).HasColumnName("cargo_id");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.Cargo).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("FK__empleados__cargo__3E52440B");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Factura__D4782DA042BFF861");

            entity.ToTable("Factura");

            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cliente_nombre");
            entity.Property(e => e.CodigoFactura)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("codigo_factura");
            entity.Property(e => e.EmpleadoId).HasColumnName("empleado_id");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.TipoVenta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_venta");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK__Factura__emplead__6D0D32F4");
        });

        modelBuilder.Entity<LoginCliente>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("PK__Login_Cl__1F5DF0A7876CA3D4");

            entity.ToTable("Login_Cliente");

            entity.HasIndex(e => e.Correo, "UQ__Login_Cl__2A586E0BB35824EF").IsUnique();

            entity.Property(e => e.Loginid)
                .ValueGeneratedNever()
                .HasColumnName("loginid");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.Correo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("correo");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__menu__3213E83F421B9D87");

            entity.ToTable("menu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.TipoMenu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_menu");
            entity.Property(e => e.TipoVenta)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo_venta");
        });

        modelBuilder.Entity<MenuCombo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__menu_com__3213E83F4E02E4C8");

            entity.ToTable("menu_combo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");

            entity.HasOne(d => d.Combo).WithMany(p => p.MenuCombos)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__menu_comb__combo__5DCAEF64");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuCombos)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__menu_comb__menu___5CD6CB2B");
        });

        modelBuilder.Entity<MenuPlato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__menu_pla__3213E83FB72F2388");

            entity.ToTable("menu_plato");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.PlatoId).HasColumnName("plato_id");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuPlatos)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK__menu_plat__menu___59063A47");

            entity.HasOne(d => d.Plato).WithMany(p => p.MenuPlatos)
                .HasForeignKey(d => d.PlatoId)
                .HasConstraintName("FK__menu_plat__plato__59FA5E80");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mesas__3213E83F145D7671");

            entity.ToTable("mesas");

            entity.HasIndex(e => e.Numero, "UQ__mesas__FC77F21129056376").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Capacidad).HasColumnName("capacidad");
            entity.Property(e => e.Disponibilidad)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("disponibilidad");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.MetodoPagoId).HasName("PK__MetodoPa__DBF399978FEB5BA7");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.MetodoPagoId)
                .ValueGeneratedNever()
                .HasColumnName("metodo_pago_id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pago__FFF0A58E8BBE3DAC");

            entity.ToTable("Pago");

            entity.Property(e => e.PagoId).HasColumnName("pago_id");
            entity.Property(e => e.FacturaId).HasColumnName("factura_id");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.MetodoPagoId).HasColumnName("metodo_pago_id");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");

            entity.HasOne(d => d.Factura).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.FacturaId)
                .HasConstraintName("FK__Pago__factura_id__71D1E811");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MetodoPagoId)
                .HasConstraintName("FK__Pago__metodo_pag__72C60C4A");
        });

        modelBuilder.Entity<PagoTarjetum>(entity =>
        {
            entity.HasKey(e => e.IdPagoTarjeta).HasName("PK__PagoTarj__DE3159A3FDC88611");

            entity.Property(e => e.IdPagoTarjeta).HasColumnName("id_PagoTarjeta");
            entity.Property(e => e.Digitos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("digitos");
            entity.Property(e => e.PagoId).HasColumnName("pago_id");
            entity.Property(e => e.Referencia)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("referencia");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
            entity.Property(e => e.Titular)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("titular");

            entity.HasOne(d => d.Pago).WithMany(p => p.PagoTarjeta)
                .HasForeignKey(d => d.PagoId)
                .HasConstraintName("FK__PagoTarje__pago___75A278F5");
        });

        modelBuilder.Entity<PedidoLocal>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido_L__6FF014897F94549E");

            entity.ToTable("Pedido_Local");

            entity.Property(e => e.IdPedido).HasColumnName("id_pedido");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Abierta")
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaApertura");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.IdMesero).HasColumnName("id_mesero");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente");
        });

        modelBuilder.Entity<Plato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__platos__3213E83F335F5164");

            entity.ToTable("platos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Imagen)
                .IsUnicode(false)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Platos)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__platos__categori__44FF419A");
        });

        modelBuilder.Entity<PlatosCombo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__platos_c__3213E83FD1FAEE4F");

            entity.ToTable("platos_combos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.PlatoId).HasColumnName("plato_id");

            entity.HasOne(d => d.Combo).WithMany(p => p.PlatosCombos)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__platos_co__combo__4AB81AF0");

            entity.HasOne(d => d.Plato).WithMany(p => p.PlatosCombos)
                .HasForeignKey(d => d.PlatoId)
                .HasConstraintName("FK__platos_co__plato__4BAC3F29");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__promocio__3213E83F30240C11");

            entity.ToTable("promociones");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("descuento");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__promocion__categ__4E88ABD4");
        });
        modelBuilder.HasSequence("seq_factura_local");
        modelBuilder.HasSequence("seq_factura_online");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
