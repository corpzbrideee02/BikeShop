<template>
  <div>
    <footer>
      best viewed on mobile device — <a href="https://corpzbrideee02.github.io/" target="_blank" rel="noopener noreferrer">Dianne Corpuz</a> &copy;{{
        new Date().getFullYear()
      }}
    </footer>
    <Toolbar>
      <template #left>
        <span style="font-family: 'Aclonica', sans-serif"
          >D.Bike Store</span
        >
      </template>
      <template #right>
        <Menu id="overlay_menu" ref="menu" :model="menuitems" :popup="true" />
        <Button icon="pi pi-bars" @click="toggle" />
      </template>
    </Toolbar>
  </div>
  <router-view />
</template>
<script>
import { ref } from "vue";
export default {
  setup() {
    const menuitems = ref([
      { label: "Home", icon: "pi pi-home", to: "/" },
      { label: "Login", icon: "pi pi-user-edit", to: "/login" },
      { label: "Register", icon: "pi pi-user-plus", to: "/register" },
      { label: "Brands", icon: "pi pi-sort-amount-down", to: "/brands" },
      { label: "Orders", icon: "pi pi-list", to: "/orders" },
      { label: "Branches", icon: "pi pi-sitemap", to: "/branch" },
      { label: "Logout", icon: "pi pi-power-off", to: "/logout" },
      /* { label: "Map", to: "/map" }, */
    ]);
    const menu = ref();
    const toggle = (event) => {
      menu.value.toggle(event);
    };
    return {
      menuitems,
      menu,
      toggle,
    };
  },
};
</script>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
