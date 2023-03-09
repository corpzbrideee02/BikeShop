<template>
  <div class="container">
    <div class="loginPageContainer">
      <div class="heading"><img src="../assets/bike.png" height="60" style="vertical-align: middle;" />Login</div>
      <div class="p-fluid">
        <div class="p-field">
          <span class="p-float-label">
            <InputText
              id="email"
              type="text"
              v-model="state.email"
              :class="{ 'p-invalid': state.validationErrors.email }"
            />
            <label for="email">Email</label>
          </span>
          <small v-show="state.validationErrors.email" class="p-error"
            >valid format is required.</small
          >
        </div>

        <div class="p-field">
          <span class="p-float-label">
            <InputText
              id="password"
              :type="state.visibility"
              v-model="state.password"
              :class="{ 'p-invalid': state.validationErrors.password }"
            />
            <span class="p-input-icon-right">
              <i
                v-if="state.visibility === 'password'"
                class="pi pi-eye"
                @click="showPassword"
              />
              <i
                v-if="state.visibility === 'text'"
                class="pi pi-eye-slash"
                @click="hidePassword"
              />
            </span>
            <label for="password">Password</label>
          </span>
          <small v-show="state.validationErrors.password" class="p-error"
            >Password is required.</small
          >
        </div>

        <Button
          label="Login"
          @click="login"
          class="p-button-outlined"
          style="margin: 5vh; width: 25vw; color: var(--teal-900)"
        />
      </div>
    </div>
      <div class="status">{{ state.status }}</div>
  </div>
</template>
<script>
import { poster } from "../util/apiutil";
import { reactive } from "vue";
import { useRouter, useRoute } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    const route = useRoute();
    let state = reactive({
      email: "",
      password: "",
      status: "",
      visibility: "password",
      validationErrors: [],
    });
    const login = async () => {
      await sessionStorage.removeItem("customer");
      try {
        !state.email.trim()
          ? (state.validationErrors["email"] = true)
          : delete state.validationErrors["email"];
        !state.password.trim()
          ? (state.validationErrors["password"] = true)
          : delete state.validationErrors["password"];
        if (Object.keys(state.validationErrors).length === 0) {
          state.status = "logging into server";
          let customerHelper = {
            firstname: "",
            lastname: "",
            email: state.email,
            password: state.password,
          };
          let payload = await poster("login", customerHelper); // in util
          if (payload.token.indexOf("failed") > 0) {
            state.status = payload.token;
          } else {
            await sessionStorage.setItem("customer", JSON.stringify(payload));
            route.params.nextUrl
              ? router.push({ name: route.params.nextUrl })
              : router.push({ name: "Home" });
          }
        }
      } catch (err) {
        console.log(`error ${err}`);
        state.status = `Error has occured: ${err}`;
      }
    };
    const showPassword = () => {
      state.visibility = "text";
    };
    const hidePassword = () => {
      state.visibility = "password";
    };
    return {
      login,
      state,
      showPassword,
      hidePassword,
    };
  },
};
</script>
