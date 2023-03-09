<template>
  <div class="container">
    <img
      style="margin-top: 2vh"
      src="../assets/cart.png"
      height="100"
    />
    <div class="heading">Cart Contents</div>
    <div class="status">{{ state.status }}</div>
    <div v-if="state.cart.length > 0" id="cart">
      <DataTable
        :value="state.cart"
        :scrollable="true"
        scrollHeight="38vh"
        dataKey="id"
        class="p-datatable-striped"
        id="cart-contents"
      >
        <Column header="Name" field="item.productName"> </Column>
        <Column header="Qty" field="qty" />
        <Column header="Price" field="item.msrp">
          <template #body="slotProps">
            {{ formatCurrency(slotProps.data.item.msrp) }}
          </template>
        </Column>
        <Column header="Extended" field="item.costPrice">
          <template #body="slotProps">
            {{ formatCurrency(slotProps.data.item.costPrice) }}
          </template>
        </Column>
      </DataTable>
    </div>
    <div v-if="state.cart.length > 0">
      <table style="margin-top: 1vh">
        <tr>
          <td style="width: 100%; text-align: right; font-weight: bold">
            Sub Total:
          </td>
          <td style="text-align: right; padding: 0 3vw">
            {{ formatCurrency(state.subTot) }}
          </td>
        </tr>

        <tr>
          <td style="width: 100%; text-align: right; font-weight: bold">
            Tax:
          </td>
          <td style="text-align: right; padding: 0 3vw">
            {{ getTax(state.subTot) }}
          </td>
        </tr>
        <tr>
          <td style="width: 100%; text-align: right; font-weight: bold">
            Total:
          </td>
          <td style="text-align: right; padding: 0 3vw">
            {{ formatCurrency(finalTotal(state.subTot)) }}
          </td>
        </tr>
      </table>
      <Button
        style="margin: 2vh; color: var(--teal-900)"
        label="Add Cart"
        @click="addCart"
        class="p-button-outlined margin-button1"
      />
      <Button
        style="margin: 2vh; color: var(--teal-900)"
        label="Empty Cart"
        @click="emptyCart"
        class="p-button-outlined margin-button1"
      />
    </div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { poster } from "../util/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    onMounted(() => {
      if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
        state.cart.map((cartItem) => {
          state.subTot += cartItem.item.costPrice * cartItem.qty;
        });
      } else {
        state.cart = [];
      }
    });
    let state = reactive({
      status: "",
      subTot: 0,
      calTot: 0,
      cart: [],
    });

    const emptyCart = () => {
      sessionStorage.removeItem("cart");
      state.cart = [];
      state.status = "cart emptied";
    };
    const formatCurrency = (value) => {
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };
    const getTax = (value) => {
      value = state.subTot * 0.13;
      return value.toLocaleString("en-US", {
        style: "currency",
        currency: "USD",
      });
    };
    const finalTotal = (subt) => {
      return subt + subt * 0.13;
    };

    const addCart = async () => {
      let customer = JSON.parse(sessionStorage.getItem("customer"));
      let ProductItem = JSON.parse(sessionStorage.getItem("cart"));
      try {
        state.status = "sending cart info to server";
        let orderHelper = { email: customer.email, selections: ProductItem };
        let payload = await poster("order", orderHelper);
        if (payload.indexOf("not") > 0) {
          state.status = payload;
        } else {
          emptyCart();
          state.status = payload;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error add cart: ${err}`;
      }
    };

    return {
      state,
      emptyCart,
      formatCurrency,
      getTax,
      finalTotal,
      addCart,
    };
  },
};
</script>
